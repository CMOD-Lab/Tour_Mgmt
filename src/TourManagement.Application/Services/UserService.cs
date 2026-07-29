using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Application.Services;

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
            ArgumentNullException.ThrowIfNull(user);
            var exists = await _userRepository.ExistsAsync(user.Email, cancellationToken);
            if (exists)
                throw new Domain.Exceptions.ValidationException($"A user with email '{user.Email}' already exists.");
            _logger.LogInformation("Registering new user: {Email}", user.Email);
            user.CreatedDate = DateTime.UtcNow;
            user.IsActive = true;
            return await _userRepository.AddAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", user?.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserInfo> UpdateUserAsync(UserInfo user, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(user);
            var existing = await _userRepository.GetByEmailAsync(user.Email, cancellationToken)
                ?? throw new NotFoundException(nameof(UserInfo), user.Email);
            _logger.LogInformation("Updating user: {Email}", user.Email);
            return await _userRepository.UpdateAsync(user, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {Email}", user?.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteUserAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _userRepository.ExistsAsync(email, cancellationToken);
            if (!exists)
                throw new NotFoundException(nameof(UserInfo), email);
            _logger.LogInformation("Deleting user: {Email}", email);
            await _userRepository.DeleteAsync(email, cancellationToken);
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
            return await _userRepository.AuthenticateAsync(email, password, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user: {Email}", email);
            throw;
        }
    }
}
