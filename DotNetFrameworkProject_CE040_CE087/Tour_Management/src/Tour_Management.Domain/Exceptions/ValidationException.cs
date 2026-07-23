namespace Tour_Management.Domain.Exceptions;

/// <summary>
/// Exception thrown when a validation rule is violated.
/// </summary>
public class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public ValidationException(IEnumerable<string> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public ValidationException(string message) : base(message)
    {
        Errors = new[] { message };
    }
}
