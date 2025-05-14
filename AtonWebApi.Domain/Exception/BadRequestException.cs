namespace Blog.Exceptions;

/// <summary>
/// Исключение, указывающее на некорректный запрос от клиента.
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}