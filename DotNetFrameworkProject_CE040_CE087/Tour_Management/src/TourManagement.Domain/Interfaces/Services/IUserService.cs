using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for User business operations.
/// Uses domain entities directly to avoid circular dependencies.
/// </summary>
public interface IUserService
{
    /// <summary>Gets all active users.</summary>
    Task<IEnumerable<UserInfo>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a user by their identifier.</summary>
    Task<UserInfo?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Gets a user by their email address.</summary>
    Task<UserInfo?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Registers a new user.</summary>
    Task<UserInfo> RegisterAsync(UserInfo user, string plainPassword, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing user.</summary>
    Task<UserInfo> UpdateAsync(UserInfo user, CancellationToken cancellationToken = default);

    /// <summary>Deletes a user by their identifier.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Authenticates a user with email and password.</summary>
    Task<UserInfo?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);

    /// <summary>Searches users by name or email.</summary>
    Task<IEnumerable<UserInfo>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
