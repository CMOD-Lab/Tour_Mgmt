using Tour_Management.Domain.DTOs;

namespace Tour_Management.Domain.Interfaces.Services;

/// <summary>
/// Service interface for UserInfo business operations.
/// </summary>
public interface IUserService
{
    /// <summary>Gets all active users asynchronously.</summary>
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a user by their identifier asynchronously.</summary>
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Gets a user by their email address asynchronously.</summary>
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Registers a new user asynchronously.</summary>
    Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing user asynchronously.</summary>
    Task<UserDto?> UpdateAsync(int id, UserUpdateDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>Deletes a user by their identifier asynchronously.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Searches users by name or email asynchronously.</summary>
    Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);

    /// <summary>Validates user login credentials asynchronously.</summary>
    Task<UserDto?> ValidateLoginAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default);
}
