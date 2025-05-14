namespace Blog.Exceptions;

/// <summary>
/// Исключение, указывающее на конфликт данных, например, попытку создать уже существующую запись.
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}