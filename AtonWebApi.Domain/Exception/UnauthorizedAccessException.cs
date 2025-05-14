namespace Blog.Exceptions;

/// <summary>
/// Исключение, указывающее на попытку несанкционированного доступа.
/// </summary>
public class UnauthorizedAccessException : Exception
{
    public UnauthorizedAccessException(string message) : base(message) { }
}