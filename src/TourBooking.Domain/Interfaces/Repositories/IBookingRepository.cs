using TourBooking.Domain.Entities;

namespace TourBooking.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Booking entity operations.
/// </summary>
public interface IBookingRepository
{
    /// <summary>Gets all bookings asynchronously.</summary>
    Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a booking by ID asynchronously.</summary>
    Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Gets bookings by user email asynchronously.</summary>
    Task<IEnumerable<Booking>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Adds a new booking asynchronously.</summary>
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing booking asynchronously.</summary>
    Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default);

    /// <summary>Deletes a booking by ID asynchronously.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Checks if a booking exists by ID asynchronously.</summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
