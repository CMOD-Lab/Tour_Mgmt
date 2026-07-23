using AutoMapper;
using Microsoft.Extensions.Logging;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Domain.Interfaces.Repositories;

namespace Tour_Management.Application.Services;

/// <summary>
/// Service for managing user operations.
/// </summary>
public class UserService
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

    /// <summary>Gets all active users.</summary>
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

    /// <summary>Gets a user by their identifier.</summary>
    public async Task<UserDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with ID {UserId}", id);
            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
            throw;
        }
    }

    /// <summary>Registers a new user.</summary>
    public async Task<UserDto> RegisterAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering new user: {Email}", dto.Email);
            var existing = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
            if (existing != null)
                throw new Domain.Exceptions.ValidationException($"A user with email '{dto.Email}' already exists.");

            var user = _mapper.Map<UserInfo>(dto);
            // Hash password before storing
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var created = await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User registered with ID {UserId}", created.UserId);
            return _mapper.Map<UserDto>(created);
        }
        catch (Domain.Exceptions.ValidationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", dto.Email);
            throw;
        }
    }

    /// <summary>Validates user login credentials.</summary>
    public async Task<UserDto?> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Login attempt for: {Email}", dto.Email);
            var user = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("Login failed - user not found: {Email}", dto.Email);
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isValid)
            {
                _logger.LogWarning("Login failed - invalid password for: {Email}", dto.Email);
                return null;
            }

            _logger.LogInformation("Login successful for: {Email}", dto.Email);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for: {Email}", dto.Email);
            throw;
        }
    }

    /// <summary>Updates an existing user.</summary>
    public async Task UpdateAsync(int id, UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with ID {UserId}", id);
            var existing = await _userRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(UserInfo), id);
            _mapper.Map(dto, existing);
            await _userRepository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("User with ID {UserId} updated successfully", id);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID {UserId}", id);
            throw;
        }
    }

    /// <summary>Deletes a user by their identifier.</summary>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with ID {UserId}", id);
            if (!await _userRepository.ExistsAsync(id, cancellationToken))
                throw new NotFoundException(nameof(UserInfo), id);
            await _userRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("User with ID {UserId} deleted successfully", id);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
            throw;
        }
    }

    /// <summary>Searches users by name or email.</summary>
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
}
