namespace AtonWebApi.Application.Dto;

public class UpdateProfileDto
{
    /// <summary>
    /// Новое имя пользователя.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Новый пол пользователя: 0 (женщина), 1 (мужчина), 2 (неизвестно).
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// Новая дата рождения пользователя.
    /// </summary>
    public DateTime? Birthday { get; set; }
}