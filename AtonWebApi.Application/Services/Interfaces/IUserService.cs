using AtonWebApi.Application.Dto;
using AtonWebApi.Domain.Entities;

namespace AtonWebApi.Application.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для управления пользователями.
/// </summary>
public interface IUserService
{
    Task CreateUser(CreateUserDto dto, string createdBy);
    Task UpdateProfile(string login, UpdateProfileDto dto, string modifiedBy);
    Task UpdatePassword(string login, string newPassword, string modifiedBy);
    Task UpdateLogin(string login, string newLogin, string modifiedBy);
    Task<IEnumerable<DbUser>> GetActiveUsers();
    Task<object> GetUserByLogin(string login);
    Task<IEnumerable<DbUser>> GetUsersOlderThan(int age);
    Task DeleteUser(string login, bool softDelete, string revokedBy);
    Task RestoreUser(string login, string modifiedBy);
}