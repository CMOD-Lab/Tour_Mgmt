using TourBooking.Domain.Entities;

namespace TourBooking.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Booking business operations.
/// </summary>
public interface IBookingService
{
    /// <summary>Gets all bookings asynchronously.</summary>
    Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a booking by ID asynchronously.</summary>
    Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Gets bookings by user email asynchronously.</summary>
    Task<IEnumerable<Booking>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Creates a new booking asynchronously.</summary>
    Task<bool> CreateAsync(Booking booking, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing booking asynchronously.</summary>
    Task<bool> UpdateAsync(Booking booking, CancellationToken cancellationToken = default);

    /// <summary>Deletes a booking by ID asynchronously.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
