namespace Blog.Exceptions;

/// <summary>
/// Исключение, указывающее на отсутствие запрашиваемого ресурса.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}