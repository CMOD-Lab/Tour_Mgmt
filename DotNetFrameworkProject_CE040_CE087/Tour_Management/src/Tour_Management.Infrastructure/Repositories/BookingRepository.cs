using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Infrastructure.Data;

namespace Tour_Management.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of the IBookingRepository.
/// </summary>
public class BookingRepository : IBookingRepository
{
    private readonly TourManagementDbContext _context;
    private readonly ILogger<BookingRepository> _logger;

    public BookingRepository(TourManagementDbContext context, ILogger<BookingRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Tour)
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all bookings from database");
            throw;
        }
    }

    public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Tour)
                .FirstOrDefaultAsync(b => b.BookingId == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving booking with ID {BookingId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Booking>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Tour)
                .Where(b => b.Email == email && b.IsActive)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookings for email {Email}", email);
            throw;
        }
    }

    public async Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync(cancellationToken);
            return booking;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding booking for tour: {TourName}", booking.TourName);
            throw;
        }
    }

    public async Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking with ID {BookingId}", booking.BookingId);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var booking = await _context.Bookings.FindAsync(new object[] { id }, cancellationToken);
            if (booking != null)
            {
                booking.IsActive = false;
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with ID {BookingId}", id);
            throw;
        }
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Bookings
                .AsNoTracking()
                .AnyAsync(b => b.BookingId == id && b.IsActive, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking existence of booking with ID {BookingId}", id);
            throw;
        }
    }

    public async Task<IEnumerable<Booking>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            var term = searchTerm.ToLower();
            return await _context.Bookings
                .AsNoTracking()
                .Include(b => b.Tour)
                .Where(b => b.IsActive &&
                    (b.TourName.ToLower().Contains(term) ||
                     b.Email.ToLower().Contains(term) ||
                     b.FirstName.ToLower().Contains(term)))
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching bookings with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
