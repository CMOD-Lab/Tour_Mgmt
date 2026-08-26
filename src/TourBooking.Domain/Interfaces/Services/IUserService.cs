using TourBooking.Domain.Entities;

namespace TourBooking.Domain.Interfaces.Services;

/// <summary>
/// Service interface for UserInfo business operations.
/// </summary>
public interface IUserService
{
    /// <summary>Gets all users asynchronously.</summary>
    Task<IEnumerable<UserInfo>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a user by email asynchronously.</summary>
    Task<UserInfo?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Registers a new user asynchronously.</summary>
    Task<bool> RegisterAsync(UserInfo user, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing user asynchronously.</summary>
    Task<bool> UpdateAsync(UserInfo user, CancellationToken cancellationToken = default);

    /// <summary>Deletes a user by email asynchronously.</summary>
    Task<bool> DeleteAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Validates user login credentials asynchronously.</summary>
    Task<UserInfo?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
