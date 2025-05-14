using AtonWebApi.Application.Dto;
using AtonWebApi.Domain.Entities;

namespace AtonWebApi.Application.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса для аутентификации пользователей.
/// </summary>
public interface IAuthService
{
    Task<string> LoginAsync(LoginDto dto);
}