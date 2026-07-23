using Tour_Management.Domain.Entities;

namespace Tour_Management.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for UserInfo entity operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>Gets all active users asynchronously.</summary>
    Task<IEnumerable<UserInfo>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a user by their identifier asynchronously.</summary>
    Task<UserInfo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Gets a user by their email address asynchronously.</summary>
    Task<UserInfo?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Adds a new user asynchronously.</summary>
    Task<UserInfo> AddAsync(UserInfo user, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing user asynchronously.</summary>
    Task UpdateAsync(UserInfo user, CancellationToken cancellationToken = default);

    /// <summary>Deletes a user by their identifier asynchronously.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Checks if a user exists by their identifier asynchronously.</summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Validates user credentials asynchronously.</summary>
    Task<UserInfo?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);

    /// <summary>Searches users by name or email asynchronously.</summary>
    Task<IEnumerable<UserInfo>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
