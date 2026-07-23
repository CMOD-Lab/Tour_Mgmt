using TourManagement.Application.DTOs;

namespace TourManagement.Application.Interfaces;

/// <summary>
/// Service interface for Tour business operations.
/// </summary>
public interface ITourService
{
    /// <summary>Gets all active tours.</summary>
    Task<IEnumerable<TourDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a tour by its identifier.</summary>
    Task<TourDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Creates a new tour.</summary>
    Task<TourDto> CreateAsync(TourCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing tour.</summary>
    Task<TourDto> UpdateAsync(int id, TourUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Deletes a tour by its identifier.</summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Searches tours by name or place.</summary>
    Task<IEnumerable<TourDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
