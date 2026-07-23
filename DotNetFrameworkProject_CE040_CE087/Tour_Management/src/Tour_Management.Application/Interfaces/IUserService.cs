using Tour_Management.Application.DTOs;

namespace Tour_Management.Application.Interfaces;

/// <summary>
/// Service interface for User business operations.
/// </summary>
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UserUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<UserDto?> AuthenticateAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default);
    Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
