using TourManagement.Application.DTOs;

namespace TourManagement.Application.Interfaces.Services;

/// <summary>
/// Service interface for User business operations.
/// </summary>
public interface IUserService
{
    /// <summary>Gets all users.</summary>
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a user by email.</summary>
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Registers a new user.</summary>
    Task<UserDto> RegisterAsync(UserCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing user.</summary>
    Task UpdateAsync(string email, UserUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Deletes a user by email.</summary>
    Task DeleteAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Validates user login credentials.</summary>
    Task<UserDto?> LoginAsync(string email, string password, CancellationToken cancellationToken = default);

    /// <summary>Searches users by name or email.</summary>
    Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
