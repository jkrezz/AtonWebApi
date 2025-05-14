using AtonWebApi.Domain.Entities;

namespace AtonWebApi.Domain.Repositories;

/// <summary>
/// Интерфейс репозитория для работы с данными пользователей в базе данных.
/// </summary>
public interface IUserRepository
{
    Task InitializeDatabase();
    Task CreateUser(DbUser user);
    Task<DbUser> GetUserByLogin(string login);
    Task<IEnumerable<DbUser>> GetActiveUsers();
    Task<IEnumerable<DbUser>> GetUsersOlderThan(int age);
    Task UpdateUser(DbUser user);
    Task DeleteUser(Guid guid);
}