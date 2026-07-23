using TourManagement.Application.DTOs;

namespace TourManagement.Application.Interfaces;

/// <summary>
/// Service interface for UserInfo business operations.
/// </summary>
public interface IUserService
{
    /// <summary>Gets all active users.</summary>
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a user by their identifier.</summary>
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Creates a new user (registration).</summary>
    Task<UserDto> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing user.</summary>
    Task<UserDto> UpdateAsync(int id, UserUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Deletes a user by their identifier.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Searches users by name or email.</summary>
    Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>Validates user credentials for login.</summary>
    Task<UserDto?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
}
