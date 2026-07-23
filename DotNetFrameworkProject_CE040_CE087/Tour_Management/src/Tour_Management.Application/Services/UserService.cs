using AutoMapper;
using Microsoft.Extensions.Logging;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Interfaces;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Domain.Interfaces.Repositories;

namespace Tour_Management.Application.Services;

/// <summary>
/// Service implementation for User business operations.
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

    public async Task<UserDto> CreateAsync(UserCreateDto createDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new user: {Email}", createDto.Email);
            var user = _mapper.Map<UserInfo>(createDto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(createDto.Password);
            var created = await _userRepository.AddAsync(user, cancellationToken);
            _logger.LogInformation("User created successfully with id {UserId}", created.UserId);
            return _mapper.Map<UserDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user: {Email}", createDto.Email);
            throw;
        }
    }

    public async Task UpdateAsync(int id, UserUpdateDto updateDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating user with id {UserId}", id);
            var existing = await _userRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(UserInfo), id);
            _mapper.Map(updateDto, existing);
            await _userRepository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("User updated successfully with id {UserId}", id);
        }
        catch (NotFoundException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with id {UserId}", id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting user with id {UserId}", id);
            var exists = await _userRepository.ExistsAsync(id, cancellationToken);
            if (!exists) throw new NotFoundException(nameof(UserInfo), id);
            await _userRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("User deleted successfully with id {UserId}", id);
        }
        catch (NotFoundException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with id {UserId}", id);
            throw;
        }
    }

    public async Task<UserDto?> AuthenticateAsync(UserLoginDto loginDto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Authenticating user: {Email}", loginDto.Email);
            var user = await _userRepository.GetByEmailAsync(loginDto.Email, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("Authentication failed: user not found for email {Email}", loginDto.Email);
                return null;
            }

            bool isValid;
            try
            {
                isValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
            }
            catch
            {
                // Fallback for plain text passwords (legacy data)
                isValid = user.Password == loginDto.Password;
            }

            if (!isValid)
            {
                _logger.LogWarning("Authentication failed: invalid password for email {Email}", loginDto.Email);
                return null;
            }

            _logger.LogInformation("User authenticated successfully: {Email}", loginDto.Email);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating user: {Email}", loginDto.Email);
            throw;
        }
    }

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
