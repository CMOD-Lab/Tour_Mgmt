using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for User business operations.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    /// <summary>Initializes a new instance of UserService.</summary>
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
    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving user with email {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with email {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto> RegisterAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Registering new user with email {Email}", dto.Email);
            if (await _userRepository.ExistsAsync(dto.Email, cancellationToken))
                throw new DuplicateEntityException(nameof(UserInfo), dto.Email);

            var user = _mapper.Map<UserInfo>(dto);
            // Hash password before storing
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var created = await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User registered successfully with email {Email}", created.Email);
            return _mapper.Map<UserDto>(created);
        }
        catch (DuplicateEntityException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user with email {Email}", dto.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(string email, UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with email {Email}", email);
            var existing = await _userRepository.GetByEmailAsync(email, cancellationToken)
                ?? throw new NotFoundException(nameof(UserInfo), email);
            _mapper.Map(dto, existing);
            await _userRepository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("User with email {Email} updated successfully", email);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with email {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with email {Email}", email);
            if (!await _userRepository.ExistsAsync(email, cancellationToken))
                throw new NotFoundException(nameof(UserInfo), email);
            await _userRepository.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("User with email {Email} deleted successfully", email);
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
    public async Task<UserDto?> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Login attempt for email {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("Login failed: user not found for email {Email}", email);
                return null;
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!isValid)
            {
                _logger.LogWarning("Login failed: invalid password for email {Email}", email);
                return null;
            }

            _logger.LogInformation("Login successful for email {Email}", email);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email {Email}", email);
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
}
