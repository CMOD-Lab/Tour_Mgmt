using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Application.Interfaces;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for User business operations.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="UserService"/>.
    /// </summary>
    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all users");
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with id {UserId}", id);
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with id {UserId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new user: {Email}", dto.Email);

            var emailExists = await _userRepository.EmailExistsAsync(dto.Email, cancellationToken);
            if (emailExists)
                throw new InvalidOperationException($"Email '{dto.Email}' is already registered.");

            var user = _mapper.Map<User>(dto);
            // Hash the password using BCrypt-style hashing via built-in methods
            user.PasswordHash = HashPassword(dto.Password);

            var created = await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User created successfully with id {UserId}", created.Id);
            return _mapper.Map<UserDto>(created);
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Email}", dto.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int id, UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with id {UserId}", id);
            var existing = await _userRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(User), id);

            _mapper.Map(dto, existing);
            await _userRepository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("User updated successfully with id {UserId}", id);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with id {UserId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with id {UserId}", id);
            var exists = await _userRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
                throw new NotFoundException(nameof(User), id);

            await _userRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("User deleted successfully with id {UserId}", id);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with id {UserId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating user: {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (user == null || !user.IsActive)
                return null;

            if (!VerifyPassword(password, user.PasswordHash))
                return null;

            _logger.LogInformation("User authenticated successfully: {Email}", email);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);
            var users = await _userRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    /// <summary>
    /// Hashes a plain-text password using SHA256.
    /// </summary>
    private static string HashPassword(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Verifies a plain-text password against a stored hash.
    /// </summary>
    private static bool VerifyPassword(string password, string storedHash)
    {
        var hash = HashPassword(password);
        return hash == storedHash;
    }
}
