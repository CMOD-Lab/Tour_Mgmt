using Microsoft.Extensions.Logging;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Exceptions;
using TourBooking.Domain.Interfaces.Repositories;
using TourBooking.Domain.Interfaces.Services;

namespace TourBooking.Application.Services;

/// <summary>
/// Service implementation for UserInfo business operations.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    /// <summary>Initializes a new instance of the <see cref="UserService"/> class.</summary>
    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserInfo>> GetAllAsync(CancellationToken cancellationToken = default)
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
    public async Task<UserInfo?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with email: {Email}", email);
            return await _userRepository.GetByEmailAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> RegisterAsync(UserInfo user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering new user with email: {Email}", user.Email);

            if (await _userRepository.ExistsAsync(user.Email, cancellationToken))
            {
                _logger.LogWarning("User with email {Email} already exists", user.Email);
                throw new DuplicateEntityException($"A user with email '{user.Email}' already exists.");
            }

            await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User registered successfully with email: {Email}", user.Email);
            return true;
        }
        catch (DuplicateEntityException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email: {Email}", user.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(UserInfo user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with email: {Email}", user.Email);

            if (!await _userRepository.ExistsAsync(user.Email, cancellationToken))
            {
                throw new NotFoundException(nameof(UserInfo), user.Email);
            }

            await _userRepository.UpdateAsync(user, cancellationToken);
            _logger.LogInformation("User updated successfully with email: {Email}", user.Email);
            return true;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with email: {Email}", user.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with email: {Email}", email);

            if (!await _userRepository.ExistsAsync(email, cancellationToken))
            {
                throw new NotFoundException(nameof(UserInfo), email);
            }

            await _userRepository.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("User deleted successfully with email: {Email}", email);
            return true;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserInfo?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Attempting login for email: {Email}", email);
            var user = await _userRepository.ValidateCredentialsAsync(email, password, cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("Login failed for email: {Email}", email);
            }
            else
            {
                _logger.LogInformation("Login successful for email: {Email}", email);
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", email);
            throw;
        }
    }
}
