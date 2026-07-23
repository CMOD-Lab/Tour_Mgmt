using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
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
    public async Task<bool> RegisterAsync(UserInfo user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering new user with email {Email}", user.Email);

            if (await _userRepository.ExistsAsync(user.Email, cancellationToken))
            {
                throw new DuplicateEntityException("UserInfo", user.Email);
            }

            await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User {Email} registered successfully", user.Email);
            return true;
        }
        catch (DuplicateEntityException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email {Email}", user.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(UserInfo user, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with email {Email}", user.Email);

            if (!await _userRepository.ExistsAsync(user.Email, cancellationToken))
            {
                throw new NotFoundException("UserInfo", user.Email);
            }

            await _userRepository.UpdateAsync(user, cancellationToken);
            _logger.LogInformation("User {Email} updated successfully", user.Email);
            return true;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with email {Email}", user.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with email {Email}", email);

            if (!await _userRepository.ExistsAsync(email, cancellationToken))
            {
                throw new NotFoundException("UserInfo", email);
            }

            await _userRepository.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("User {Email} deleted successfully", email);
            return true;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with email {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserInfo?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating user with email {Email}", email);
            var user = await _userRepository.ValidateCredentialsAsync(email, password, cancellationToken);
            if (user != null)
            {
                _logger.LogInformation("User {Email} authenticated successfully", email);
            }
            else
            {
                _logger.LogWarning("Authentication failed for user {Email}", email);
            }
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user with email {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserInfo>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching users with term {SearchTerm}", searchTerm);
            return await _userRepository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term {SearchTerm}", searchTerm);
            throw;
        }
    }
}
