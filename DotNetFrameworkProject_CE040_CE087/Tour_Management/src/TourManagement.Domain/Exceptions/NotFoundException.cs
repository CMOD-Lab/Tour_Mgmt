namespace TourManagement.Domain.Exceptions;

/// <summary>
/// Exception thrown when an entity is not found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="NotFoundException"/>.
    /// </summary>
    /// <param name="entityName">The name of the entity.</param>
    /// <param name="id">The identifier that was not found.</param>
    public NotFoundException(string entityName, object id)
        : base($"{entityName} with id '{id}' was not found.")
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="NotFoundException"/> with a custom message.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public NotFoundException(string message) : base(message)
    {
    }
}
