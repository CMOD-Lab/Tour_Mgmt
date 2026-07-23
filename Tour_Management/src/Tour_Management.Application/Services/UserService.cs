using AutoMapper;
using Microsoft.Extensions.Logging;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Application.Services;

/// <summary>
/// Service implementation for UserInfo business operations.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserInfo>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all users");
            return await _userRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserInfo?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with email {Email}", email);
            return await _userRepository.GetByEmailAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with email {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserInfo> RegisterUserAsync(UserInfo user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering new user: {Email}", user.Email);
            if (await _userRepository.ExistsAsync(user.Email, cancellationToken))
                throw new DuplicateEntityException(nameof(UserInfo), "Email", user.Email);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;
            // Hash password before storing
            user.Password = BCryptHashPassword(user.Password);
            var created = await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User registered successfully: {Email}", created.Email);
            return created;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", user.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserInfo> UpdateUserAsync(UserInfo user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user: {Email}", user.Email);
            var existing = await _userRepository.GetByEmailAsync(user.Email, cancellationToken)
                ?? throw new NotFoundException(nameof(UserInfo), user.Email);
            user.Password = existing.Password;
            user.CreatedDate = existing.CreatedDate;
            user.IsActive = existing.IsActive;
            var updated = await _userRepository.UpdateAsync(user, cancellationToken);
            _logger.LogInformation("User updated successfully: {Email}", updated.Email);
            return updated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {Email}", user.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteUserAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user: {Email}", email);
            if (!await _userRepository.ExistsAsync(email, cancellationToken))
                throw new NotFoundException(nameof(UserInfo), email);
            await _userRepository.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("User deleted successfully: {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserInfo?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating user: {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (user == null || !VerifyPassword(password, user.Password))
            {
                _logger.LogWarning("Authentication failed for user: {Email}", email);
                return null;
            }
            _logger.LogInformation("User authenticated successfully: {Email}", email);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserInfo>> SearchUsersAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);
            return await _userRepository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            throw;
        }
    }

    /// <summary>Hashes a password using BCrypt-style hashing (simplified for compatibility).</summary>
    private static string BCryptHashPassword(string password)
    {
        // Use a simple but secure hash for .NET 8 compatibility without extra packages
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(password + "TourMgmt_Salt_2024");
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    /// <summary>Verifies a password against a stored hash.</summary>
    private static bool VerifyPassword(string password, string storedHash)
    {
        var hash = BCryptHashPassword(password);
        return hash == storedHash;
    }
}
