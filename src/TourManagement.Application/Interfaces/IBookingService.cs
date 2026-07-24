using TourManagement.Application.DTOs;

namespace TourManagement.Application.Interfaces;

/// <summary>
/// Service interface for Booking business operations.
/// </summary>
public interface IBookingService
{
    /// <summary>Gets all bookings asynchronously.</summary>
    Task<IEnumerable<BookingDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a booking by ID asynchronously.</summary>
    Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Gets all bookings for a specific user email asynchronously.</summary>
    Task<IEnumerable<BookingDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Creates a new booking asynchronously.</summary>
    Task<bool> CreateAsync(BookingCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Deletes a booking by ID asynchronously.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
