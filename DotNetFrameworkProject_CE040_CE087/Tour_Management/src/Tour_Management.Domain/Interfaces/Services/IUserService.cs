using Tour_Management.Domain.DTOs;

namespace Tour_Management.Domain.Interfaces.Services;

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
    Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing user.</summary>
    Task<UserDto> UpdateAsync(int id, UserUpdateDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>Deletes a user by their identifier.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Searches users by name or email.</summary>
    Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>Validates user login credentials.</summary>
    Task<UserDto?> ValidateLoginAsync(string email, string password, CancellationToken cancellationToken = default);
}
