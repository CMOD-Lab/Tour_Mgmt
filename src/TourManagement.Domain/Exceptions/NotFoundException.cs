namespace TourManagement.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>Initializes a new instance of <see cref="NotFoundException"/>.</summary>
    public NotFoundException(string entityName, object key)
        : base($"Entity '{entityName}' with key '{key}' was not found.")
    {
    }

    /// <summary>Initializes a new instance of <see cref="NotFoundException"/> with a custom message.</summary>
    public NotFoundException(string message) : base(message)
    {
    }
}
