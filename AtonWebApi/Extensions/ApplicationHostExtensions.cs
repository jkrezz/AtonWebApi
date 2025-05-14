using AtonWebApi.Application.Services;
using AtonWebApi.Application.Services.Interfaces;
using AtonWebApi.Domain.Repositories;
using AtonWebApi.Infrastructure.Repositories;

namespace AtonWebApi.Extensions;

public static class ApplicationHostExtensions
{
    /// <summary>
    /// Добавляет сервисы приложения в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}