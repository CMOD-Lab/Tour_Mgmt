namespace Tour_Management.Domain.Exceptions;

/// <summary>
/// Exception thrown when an entity is not found.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="entityName">The name of the entity that was not found.</param>
    /// <param name="id">The identifier that was searched for.</param>
    public NotFoundException(string entityName, object id)
        : base($"{entityName} with id '{id}' was not found.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public NotFoundException(string message) : base(message)
    {
    }
}
