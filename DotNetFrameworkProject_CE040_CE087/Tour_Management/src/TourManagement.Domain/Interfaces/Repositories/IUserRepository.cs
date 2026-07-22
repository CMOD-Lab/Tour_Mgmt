using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for User entity operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>Gets all active users.</summary>
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a user by its identifier.</summary>
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Gets a user by email address.</summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Adds a new user.</summary>
    Task<User> AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing user.</summary>
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>Deletes a user by its identifier.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Checks if a user exists by its identifier.</summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Checks if an email is already registered.</summary>
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Searches users by name or email.</summary>
    Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
