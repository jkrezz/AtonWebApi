using AtonWebApi.Application.Dto;
using AtonWebApi.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AtonWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Создаёт нового пользователя. Требуется роль администратора.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Неверные данные запроса");

            await _userService.CreateUser(dto, User.Identity.Name);
            return CreatedAtAction(nameof(GetUserByLogin), new { login = dto.Login }, dto);
        }

        /// <summary>
        /// Обновляет профиль пользователя (имя, пол, дата рождения).
        /// </summary>
        [HttpPut("{login}/profile")]
        public async Task<IActionResult> UpdateProfile(string login, [FromBody] UpdateProfileDto dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Неверные данные запроса");

            await _userService.UpdateProfile(login, dto, User.Identity.Name);
            var updatedUser = await _userService.GetUserByLogin(login);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Обновляет пароль пользователя.
        /// </summary>
        [HttpPut("{login}/password")]
        public async Task<IActionResult> UpdatePassword(string login, [FromBody] UpdatePasswordDto dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Неверные данные запроса");

            await _userService.UpdatePassword(login, dto.NewPassword, User.Identity.Name);
            return Ok();
        }

        /// <summary>
        /// Обновляет логин пользователя.
        /// </summary>
        [HttpPut("{login}/login")]
        public async Task<IActionResult> UpdateLogin(string login, [FromBody] UpdateLoginDto dto)
        {
            if (!ModelState.IsValid)
                throw new ArgumentException("Неверные данные запроса");

            await _userService.UpdateLogin(login, dto.NewLogin, User.Identity.Name);
            var updatedUser = await _userService.GetUserByLogin(dto.NewLogin);
            return Ok(updatedUser);
        }

        /// <summary>
        /// Возвращает список всех активных пользователей, отсортированных по дате создания. Требуется роль администратора.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllActiveUsers()
        {
            var users = await _userService.GetActiveUsers();
            return Ok(users.OrderBy(u => u.CreatedOn));
        }

        /// <summary>
        /// Возвращает данные пользователя по логину. Требуется роль администратора.
        /// </summary>
        [HttpGet("{login}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserByLogin(string login)
        {
            var user = await _userService.GetUserByLogin(login);
            return Ok(user);
        }

        /// <summary>
        /// Возвращает список пользователей старше указанного возраста. Требуется роль администратора.
        /// </summary>
        [HttpGet("older-than/{age}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersOlderThan(int age)
        {
            var users = await _userService.GetUsersOlderThan(age);
            return Ok(users);
        }

        /// <summary>
        /// Удаляет пользователя (мягкое или полное удаление). Требуется роль администратора.
        /// </summary>
        [HttpDelete("{login}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string login, [FromQuery] bool softDelete = true)
        {
            await _userService.DeleteUser(login, softDelete, User.Identity.Name);
            return NoContent();
        }

        /// <summary>
        /// Восстанавливает ранее мягко удалённого пользователя. Требуется роль администратора.
        /// </summary>
        [HttpPut("{login}/restore")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreUser(string login)
        {
            await _userService.RestoreUser(login, User.Identity.Name);
            var restoredUser = await _userService.GetUserByLogin(login);
            return Ok(restoredUser);
        }
    }
}