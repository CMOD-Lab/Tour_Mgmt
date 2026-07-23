using Tour_Management.Domain.Entities;

namespace Tour_Management.Domain.Interfaces.Repositories;

/// <summary>
/// Repository interface for Booking entity operations.
/// </summary>
public interface IBookingRepository
{
    /// <summary>Gets all active bookings asynchronously.</summary>
    Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a booking by its identifier asynchronously.</summary>
    Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Gets all bookings for a specific user email asynchronously.</summary>
    Task<IEnumerable<Booking>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>Adds a new booking asynchronously.</summary>
    Task<Booking> AddAsync(Booking booking, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing booking asynchronously.</summary>
    Task UpdateAsync(Booking booking, CancellationToken cancellationToken = default);

    /// <summary>Deletes a booking by its identifier asynchronously.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Checks if a booking exists by its identifier asynchronously.</summary>
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Searches bookings by tour name or email asynchronously.</summary>
    Task<IEnumerable<Booking>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
