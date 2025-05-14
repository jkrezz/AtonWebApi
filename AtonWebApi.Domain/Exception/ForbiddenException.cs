namespace Blog.Exceptions;

/// <summary>
/// Исключение, указывающее на отсутствие у пользователя прав для выполнения действия.
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException(string message) : base(message) { }
}