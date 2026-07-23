using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for user operations.
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
    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all users.");
            var users = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users.");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with email {Email}.", email);
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            return user is null ? null : _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with email {Email}.", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto?> AuthenticateAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating user with email {Email}.", email);
            var user = await _userRepository.AuthenticateAsync(email, password, cancellationToken);
            return user is null ? null : _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user with email {Email}.", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto> RegisterAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering new user with email {Email}.", dto.Email);

            var exists = await _userRepository.ExistsAsync(dto.Email, cancellationToken);
            if (exists)
            {
                throw new DuplicateEntityException(nameof(UserInfo), "Email", dto.Email);
            }

            var user = _mapper.Map<UserInfo>(dto);
            await _userRepository.AddAsync(user, cancellationToken);

            _logger.LogInformation("User {Email} registered successfully.", dto.Email);
            return _mapper.Map<UserDto>(user);
        }
        catch (DuplicateEntityException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email {Email}.", dto.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto> UpdateAsync(string email, UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with email {Email}.", email);

            var user = await _userRepository.GetByEmailAsync(email, cancellationToken)
                ?? throw new NotFoundException(nameof(UserInfo), email);

            _mapper.Map(dto, user);
            await _userRepository.UpdateAsync(user, cancellationToken);

            _logger.LogInformation("User {Email} updated successfully.", email);
            return _mapper.Map<UserDto>(user);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with email {Email}.", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with email {Email}.", email);

            var exists = await _userRepository.ExistsAsync(email, cancellationToken);
            if (!exists)
            {
                throw new NotFoundException(nameof(UserInfo), email);
            }

            await _userRepository.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("User {Email} deleted successfully.", email);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with email {Email}.", email);
            throw;
        }
    }
}
