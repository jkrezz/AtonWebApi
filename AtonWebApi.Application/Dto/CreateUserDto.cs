namespace AtonWebApi.Application.Dto;

public class CreateUserDto
{
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Пол пользователя: 0 (женщина), 1 (мужчина), 2 (неизвестно).
    /// </summary>
    public int Gender { get; set; }

    /// <summary>
    /// Дата рождения пользователя.
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// Указывает, является ли пользователь администратором.
    /// </summary>
    public bool IsAdmin { get; set; }
}