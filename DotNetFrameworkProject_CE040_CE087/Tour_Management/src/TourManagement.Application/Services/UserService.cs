using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for UserInfo business operations.
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
            return user is null ? null : _mapper.Map<UserDto>(user);
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
            var user = _mapper.Map<UserInfo>(dto);
            // Hash the password before storing
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var created = await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User created with id {UserId}", created.Id);
            return _mapper.Map<UserDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Email}", dto.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserDto> UpdateAsync(int id, UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with id {UserId}", id);
            var existing = await _userRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(UserInfo), id);

            _mapper.Map(dto, existing);
            var updated = await _userRepository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("User updated with id {UserId}", id);
            return _mapper.Map<UserDto>(updated);
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
                throw new NotFoundException(nameof(UserInfo), id);

            await _userRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("User deleted with id {UserId}", id);
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

    /// <inheritdoc/>
    public async Task<UserDto?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Validating credentials for email: {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
            if (user is null)
                return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            if (!isValid)
                return null;

            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating credentials for email: {Email}", email);
            throw;
        }
    }
}
