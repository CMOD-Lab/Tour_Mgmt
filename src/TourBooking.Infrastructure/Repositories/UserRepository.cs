using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Interfaces.Repositories;
using TourBooking.Infrastructure.Data;

namespace TourBooking.Infrastructure.Repositories;

/// <summary>
/// EF Core repository implementation for UserInfo entity.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly TourBookingDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    /// <summary>Initializes a new instance of the <see cref="UserRepository"/> class.</summary>
    public UserRepository(TourBookingDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserInfo>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.UserInfos
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users from database");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserInfo?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.UserInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task AddAsync(UserInfo user, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.UserInfos.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding user with email: {Email}", user.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(UserInfo user, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.UserInfos.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with email: {Email}", user.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _context.UserInfos.FindAsync(new object[] { email }, cancellationToken);
            if (user != null)
            {
                _context.UserInfos.Remove(user);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.UserInfos
                .AsNoTracking()
                .AnyAsync(u => u.Email == email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of user with email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<UserInfo?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.UserInfos
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating credentials for email: {Email}", email);
            throw;
        }
    }
}
