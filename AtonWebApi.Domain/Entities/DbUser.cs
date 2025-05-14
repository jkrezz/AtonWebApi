using System.ComponentModel.DataAnnotations;

namespace AtonWebApi.Domain.Entities;

public class DbUser
{
    /// <summary>
    /// Уникальный идентификатор пользователя.
    /// </summary>
    public Guid Guid { get; set; }
    
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Хэшированный пароль пользователя..
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [Required]
    [RegularExpression("^[a-zA-Zа-яА-ЯёЁ]+$")]
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
    public bool Admin { get; set; }
    
    /// <summary>
    /// Дата создания пользователя.
    /// </summary>
    public DateTime CreatedOn { get; set; }
    
    /// <summary>
    /// Логин пользователя, создавшего запись.
    /// </summary>
    public string CreatedBy { get; set; }
    
    /// <summary>
    /// Дата последнего изменения пользователя.
    /// </summary>
    public DateTime? ModifiedOn { get; set; }
    
    /// <summary>
    /// Логин пользователя, выполнившего последнее изменение.
    /// </summary>
    public string ModifiedBy { get; set; }
    
    /// <summary>
    /// Дата деактивации пользователя (мягкое удаление).
    /// </summary>
    public DateTime? RevokedOn { get; set; }
    
    /// <summary>
    /// Логин пользователя, выполнившего деактивацию.
    /// </summary>
    public string RevokedBy { get; set; }
}