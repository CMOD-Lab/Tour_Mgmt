namespace TourManagement.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}

/// <summary>
/// Exception thrown when a validation error occurs.
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

/// <summary>
/// Exception thrown when a duplicate entity is detected.
/// </summary>
public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string entityName, object key)
        : base($"{entityName} with key '{key}' already exists.")
    {
    }
}
