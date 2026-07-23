namespace Tour_Management.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested entity is not found.
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
        EntityName = entityName;
        Id = id;
    }

    /// <summary>Gets the name of the entity that was not found.</summary>
    public string EntityName { get; }

    /// <summary>Gets the identifier that was searched for.</summary>
    public object Id { get; }
}
