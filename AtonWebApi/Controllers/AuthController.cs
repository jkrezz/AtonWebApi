using AtonWebApi.Application.Dto;
using AtonWebApi.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AtonWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Аутентифицирует пользователя и возвращает JWT-токен.
        /// </summary>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Неверные данные запроса");

            var token = await _authService.LoginAsync(dto);
            return Ok(new { Token = token });
        }
    }
}