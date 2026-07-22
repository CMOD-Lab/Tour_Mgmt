using TourManagement.Application.DTOs;

namespace TourManagement.Application.Interfaces;

/// <summary>
/// Service interface for User business operations.
/// </summary>
public interface IUserService
{
    /// <summary>Gets all active users.</summary>
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a user by its identifier.</summary>
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Creates a new user (registration).</summary>
    Task<UserDto> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing user.</summary>
    Task UpdateAsync(int id, UserUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Deletes a user by its identifier.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Authenticates a user by email and password.</summary>
    Task<UserDto?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default);

    /// <summary>Searches users by name or email.</summary>
    Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
