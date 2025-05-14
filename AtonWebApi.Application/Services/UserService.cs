using System.Security;
using System.Text.RegularExpressions;
using AtonWebApi.Application.Dto;
using AtonWebApi.Application.Services.Interfaces;
using AtonWebApi.Domain.Entities;
using AtonWebApi.Domain.Repositories;

namespace AtonWebApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        /// <summary>
        /// Создаёт нового пользователя.
        /// </summary>
        /// <param name="dto">DTO с данными для создания пользователя.</param>
        /// <param name="createdBy">Логин пользователя, создающего запись.</param>
        public async Task CreateUser(CreateUserDto dto, string createdBy)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Login) || string.IsNullOrWhiteSpace(dto.Password))
                throw new ArgumentException("Данные пользователя не могут быть пустыми");

            if (await _userRepository.GetUserByLogin(dto.Login) != null)
                throw new ArgumentException("Логин уже существует");

            if (!Regex.IsMatch(dto.Login, @"^[a-zA-Z0-9]+$") || 
                !Regex.IsMatch(dto.Password, @"^[a-zA-Z0-9]+$"))
                throw new ArgumentException("Логин должен содержать только латинские буквы и цифры");

            if (!Regex.IsMatch(dto.Name, @"^[a-zA-Zа-яА-ЯёЁ]+$"))
                throw new ArgumentException("Имя должно содержать только латинские или русские буквы");
            
            var user = new DbUser
            {
                Guid = Guid.NewGuid(),
                Login = dto.Login,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                Gender = dto.Gender,
                Birthday = dto.Birthday,
                Admin = dto.IsAdmin,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = createdBy
            };

            await _userRepository.CreateUser(user);
        }

        /// <summary>
        /// Обновляет профиль пользователя (имя, пол, дата рождения).
        /// </summary>
        /// <param name="login">Логин пользователя для обновления.</param>
        /// <param name="dto">DTO с обновлёнными данными профиля.</param>
        /// <param name="modifiedBy">Логин пользователя, выполняющего обновление.</param>
        public async Task UpdateProfile(string login, UpdateProfileDto dto, string modifiedBy)
        {
            if (dto == null)
                throw new ArgumentException("Данные профиля не могут быть пустыми");
            
            if (dto.Name != null && !Regex.IsMatch(dto.Name, @"^[a-zA-Zа-яА-ЯёЁ]+$"))
                throw new ArgumentException("Имя должно содержать только латинские или русские буквы");
            
            if (dto.Gender.HasValue && (dto.Gender < 0 || dto.Gender > 2))
                throw new ArgumentException("Пол должен быть 0 (женщина), 1 (мужчина) или 2 (неизвестно)");

            var currentUser = await _userRepository.GetUserByLogin(modifiedBy);
            var targetUser = await _userRepository.GetUserByLogin(login);

            ValidateUserForUpdate(targetUser, currentUser, login);

            targetUser.Name = dto.Name ?? targetUser.Name;
            targetUser.Gender = dto.Gender ?? targetUser.Gender;
            targetUser.Birthday = dto.Birthday ?? targetUser.Birthday;
            targetUser.ModifiedOn = DateTime.UtcNow;
            targetUser.ModifiedBy = modifiedBy;

            await _userRepository.UpdateUser(targetUser);
        }

        /// <summary>
        /// Обновляет пароль пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя для обновления.</param>
        /// <param name="newPassword">Новый пароль.</param>
        /// <param name="modifiedBy">Логин пользователя, выполняющего обновление.</param>
        public async Task UpdatePassword(string login, string newPassword, string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("Новый пароль не может быть пустым");
            
            if (!Regex.IsMatch(newPassword, @"^[a-zA-Z0-9]+$"))
                throw new ArgumentException("Пароль должен содержать только латинские буквы и цифры");

            var currentUser = await _userRepository.GetUserByLogin(modifiedBy);
            var targetUser = await _userRepository.GetUserByLogin(login);

            ValidateUserForUpdate(targetUser, currentUser, login);

            targetUser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword); // Хэширование пароля
            targetUser.ModifiedOn = DateTime.UtcNow;
            targetUser.ModifiedBy = modifiedBy;

            await _userRepository.UpdateUser(targetUser);
        }

        /// <summary>
        /// Обновляет логин пользователя.
        /// </summary>
        /// <param name="login">Текущий логин пользователя.</param>
        /// <param name="newLogin">Новый логин.</param>
        /// <param name="modifiedBy">Логин пользователя, выполняющего обновление.</param>
        public async Task UpdateLogin(string login, string newLogin, string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(newLogin))
                throw new ArgumentException("Новый логин не может быть пустым");
            
            if (!Regex.IsMatch(newLogin, @"^[a-zA-Z0-9]+$"))
                throw new ArgumentException("Логин должен содержать только латинские буквы и цифры");

            var currentUser = await _userRepository.GetUserByLogin(modifiedBy);
            var targetUser = await _userRepository.GetUserByLogin(login);

            ValidateUserForUpdate(targetUser, currentUser, login);

            if (await _userRepository.GetUserByLogin(newLogin) != null)
                throw new ArgumentException("Новый логин уже существует");

            targetUser.Login = newLogin;
            targetUser.ModifiedOn = DateTime.UtcNow;
            targetUser.ModifiedBy = modifiedBy;

            await _userRepository.UpdateUser(targetUser);
        }

        /// <summary>
        /// Возвращает список всех активных пользователей.
        /// </summary>
        /// <returns>Коллекция активных пользователей.</returns>
        public async Task<IEnumerable<DbUser>> GetActiveUsers()
        {
            return await _userRepository.GetActiveUsers();
        }

        /// <summary>
        /// Возвращает данные пользователя по логину.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>Объект с данными пользователя.</returns>
        public async Task<object> GetUserByLogin(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Логин не может быть пустым");

            var user = await _userRepository.GetUserByLogin(login)
                ?? throw new KeyNotFoundException("Пользователь не найден");

            return new
            {
                user.Name,
                user.Gender,
                user.Birthday,
                IsActive = user.RevokedOn == null
            };
        }

        /// <summary>
        /// Возвращает список пользователей старше указанного возраста.
        /// </summary>
        /// <param name="age">Возраст для фильтрации.</param>
        /// <returns>Коллекция пользователей старше указанного возраста.</returns>
        public async Task<IEnumerable<DbUser>> GetUsersOlderThan(int age)
        {
            if (age < 0)
                throw new ArgumentException("Возраст не может быть отрицательным");

            return await _userRepository.GetUsersOlderThan(age);
        }

        /// <summary>
        /// Удаляет пользователя (мягкое или полное удаление).
        /// </summary>
        /// <param name="login">Логин пользователя для удаления.</param>
        /// <param name="softDelete">Если true, выполняется мягкое удаление; если false, полное.</param>
        /// <param name="revokedBy">Логин пользователя, выполняющего удаление.</param>
        public async Task DeleteUser(string login, bool softDelete, string revokedBy)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Логин не может быть пустым");

            var user = await _userRepository.GetUserByLogin(login)
                ?? throw new KeyNotFoundException("Пользователь не найден");

            if (softDelete)
            {
                user.RevokedOn = DateTime.UtcNow;
                user.RevokedBy = revokedBy;
                await _userRepository.UpdateUser(user);
            }
            else
            {
                await _userRepository.DeleteUser(user.Guid);
            }
        }

        /// <summary>
        /// Восстанавливает ранее мягко удалённого пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя для восстановления.</param>
        /// <param name="modifiedBy">Логин пользователя, выполняющего восстановление.</param>
        public async Task RestoreUser(string login, string modifiedBy)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Логин не может быть пустым");

            var user = await _userRepository.GetUserByLogin(login)
                ?? throw new KeyNotFoundException("Пользователь не найден");

            if (user.RevokedOn == null)
                throw new InvalidOperationException("Пользователь не деактивирован");

            user.RevokedOn = null;
            user.RevokedBy = null;
            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            await _userRepository.UpdateUser(user);
        }

        /// <summary>
        /// Проверяет возможность обновления пользователя.
        /// </summary>
        /// <param name="targetUser">Пользователь, которого нужно обновить.</param>
        /// <param name="currentUser">Текущий пользователь, выполняющий действие.</param>
        /// <param name="login">Логин пользователя для обновления.</param>
        private void ValidateUserForUpdate(DbUser targetUser, DbUser currentUser, string login)
        {
            if (targetUser == null)
                throw new KeyNotFoundException("Пользователь не найден");

            if (targetUser.RevokedOn != null)
                throw new InvalidOperationException("Нельзя модифицировать деактивированного пользователя");

            if (!currentUser.Admin && currentUser.Login != login)
                throw new SecurityException("Доступ запрещен");
        }
    }
}