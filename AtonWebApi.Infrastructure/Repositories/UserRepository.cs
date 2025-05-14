using System.Data.Common;
using AtonWebApi.Domain.Entities;
using AtonWebApi.Domain.Repositories;
using AtonWebApi.Infrastructure.Repositories.Scripts.Account;
using Dapper;
using Npgsql;

namespace AtonWebApi.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;
    private readonly string InitializeDatabaseQuery;
    private readonly string CreateUserQuery;
    private readonly string GetUserByLoginQuery;
    private readonly string GetActiveUsersQuery;
    private readonly string GetUsersOlderThanQuery;
    private readonly string UpdateUserQuery;
    private readonly string DeleteUserQuery;

    /// <summary>
    /// Инициализирует новый экземпляр класса.
    /// </summary>
    /// <param name="configuration">Конфигурация приложения, содержащая строку подключения.</param>
    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("UsersDbConnection")
                            ?? throw new ArgumentNullException(nameof(configuration),
                                "Строка подключения 'UsersDbConnection' не найдена.");

        CreateUserQuery = User.CreateUser;
        GetUserByLoginQuery = User.GetUserByLogin;
        GetActiveUsersQuery = User.GetActiveUsers;
        GetUsersOlderThanQuery = User.GetUsersOlderThan;
        UpdateUserQuery = User.UpdateUser;
        DeleteUserQuery = User.DeleteUser;

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword("admin");
        InitializeDatabaseQuery = User.InitializeDatabase.Replace("{hashedPassword}", hashedPassword);
    }

    /// <summary>
    /// Создаёт новое соединение с базой данных PostgreSQL.
    /// </summary>
    /// <returns>Объект соединения с базой данных.</returns>
    private DbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

    /// <summary>
    /// Инициализирует базу данных, создавая таблицу пользователей и добавляя начальную запись администратора.
    /// </summary>
    public async Task InitializeDatabase()
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(InitializeDatabaseQuery);
    }

    /// <summary>
    /// Создаёт нового пользователя в базе данных.
    /// </summary>
    /// <param name="user">Объект пользователя для создания.</param>
    public async Task CreateUser(DbUser user)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(CreateUserQuery, user);
    }

    /// <summary>
    /// Получает пользователя по логину.
    /// </summary>
    /// <param name="login">Логин пользователя.</param>
    /// <returns>Объект пользователя или null, если пользователь не найден.</returns>
    public async Task<DbUser> GetUserByLogin(string login)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<DbUser>(GetUserByLoginQuery, new { Login = login });
    }

    /// <summary>
    /// Получает список всех активных пользователей.
    /// </summary>
    /// <returns>Коллекция активных пользователей.</returns>
    public async Task<IEnumerable<DbUser>> GetActiveUsers()
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryAsync<DbUser>(GetActiveUsersQuery);
    }

    /// <summary>
    /// Получает список пользователей старше указанного возраста.
    /// </summary>
    /// <param name="age">Возраст для фильтрации.</param>
    /// <returns>Коллекция пользователей старше указанного возраста.</returns>
    public async Task<IEnumerable<DbUser>> GetUsersOlderThan(int age)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryAsync<DbUser>(GetUsersOlderThanQuery, new { Age = age });
    }

    /// <summary>
    /// Обновляет данные пользователя в базе данных.
    /// </summary>
    /// <param name="user">Объект пользователя с обновлёнными данными.</param>
    public async Task UpdateUser(DbUser user)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(UpdateUserQuery, user);
    }

    /// <summary>
    /// Удаляет пользователя из базы данных по его идентификатору.
    /// </summary>
    /// <param name="guid">Уникальный идентификатор пользователя.</param>
    public async Task DeleteUser(Guid guid)
    {
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(DeleteUserQuery, new { Guid = guid });
    }
}