namespace AtonWebApi.Application.Dto;

public class JwtSettings
{
    /// <summary>
    /// Секретный ключ для подписи JWT-токена.
    /// </summary>
    public string SecretKey { get; set; }

    /// <summary>
    /// Издатель токена.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Аудитория токена.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Время жизни токена.
    /// </summary>
    public int ExpiryMinutes { get; set; }
}