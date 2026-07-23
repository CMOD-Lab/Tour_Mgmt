using Tour_Management.Domain.DTOs;

namespace Tour_Management.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Booking business operations.
/// </summary>
public interface IBookingService
{
    /// <summary>Gets all active bookings asynchronously.</summary>
    Task<IEnumerable<BookingDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a booking by its identifier asynchronously.</summary>
    Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Gets all bookings for a specific user email asynchronously.</summary>
    Task<IEnumerable<BookingDto>> GetByUserEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Gets all bookings for a specific tour asynchronously.</summary>
    Task<IEnumerable<BookingDto>> GetByTourIdAsync(int tourId, CancellationToken cancellationToken = default);

    /// <summary>Creates a new booking asynchronously.</summary>
    Task<BookingDto> CreateAsync(BookingCreateDto createDto, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing booking asynchronously.</summary>
    Task<BookingDto?> UpdateAsync(int id, BookingUpdateDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>Deletes a booking by its identifier asynchronously.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Searches bookings by tour name or email asynchronously.</summary>
    Task<IEnumerable<BookingDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
