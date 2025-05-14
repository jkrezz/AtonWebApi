using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AtonWebApi.Application.Dto;
using AtonWebApi.Application.Services.Interfaces;
using AtonWebApi.Domain.Entities;
using AtonWebApi.Domain.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AtonWebApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(IUserRepository userRepository, IOptions<JwtSettings> jwtSettings)
        {
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        /// Аутентифицирует пользователя и возвращает JWT-токен.
        /// </summary>
        /// <param name="dto">DTO с логином и паролем пользователя.</param>
        /// <returns>JWT-токен в виде строки.</returns>
        public async Task<string> LoginAsync(LoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Данные для входа не могут быть пустыми");

            var user = await _userRepository.GetUserByLogin(dto.Login);

            ValidateCredentials(user, dto);

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Проверяет учетные данные пользователя.
        /// </summary>
        /// <param name="user">Объект пользователя из базы данных.</param>
        /// <param name="dto">DTO с логином и паролем.</param>
        private void ValidateCredentials(DbUser user, LoginDto dto)
        {
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password) || user.RevokedOn != null)
            {
                throw new UnauthorizedAccessException("Неверные учетные данные");
            }
        }

        /// <summary>
        /// Генерирует JWT-токен для пользователя.
        /// </summary>
        /// <param name="user">Объект пользователя.</param>
        /// <returns>JWT-токен в виде строки.</returns>
        private string GenerateJwtToken(DbUser user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

            var credentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Admin ? "Admin" : "User"),
                new Claim("UserId", user.Guid.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}