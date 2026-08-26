namespace TourBooking.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="NotFoundException"/> class.</summary>
    public NotFoundException(string message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="NotFoundException"/> class with entity details.</summary>
    public NotFoundException(string entityName, object key)
        : base($"Entity '{entityName}' with key '{key}' was not found.") { }
}

/// <summary>
/// Exception thrown when a duplicate entity is detected.
/// </summary>
public class DuplicateEntityException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="DuplicateEntityException"/> class.</summary>
    public DuplicateEntityException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when a validation error occurs.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>Gets the validation errors.</summary>
    public IEnumerable<string> Errors { get; }

    /// <summary>Initializes a new instance of the <see cref="ValidationException"/> class.</summary>
    public ValidationException(IEnumerable<string> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}
