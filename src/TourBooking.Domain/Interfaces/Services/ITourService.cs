using TourBooking.Domain.Entities;

namespace TourBooking.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Tour business operations.
/// </summary>
public interface ITourService
{
    /// <summary>Gets all tours asynchronously.</summary>
    Task<IEnumerable<Tour>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a tour by ID asynchronously.</summary>
    Task<Tour?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Creates a new tour asynchronously.</summary>
    Task<bool> CreateAsync(Tour tour, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing tour asynchronously.</summary>
    Task<bool> UpdateAsync(Tour tour, CancellationToken cancellationToken = default);

    /// <summary>Deletes a tour by ID asynchronously.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Searches tours by name or place asynchronously.</summary>
    Task<IEnumerable<Tour>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
