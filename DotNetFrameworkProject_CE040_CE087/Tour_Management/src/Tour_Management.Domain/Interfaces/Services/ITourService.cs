using Tour_Management.Domain.DTOs;

namespace Tour_Management.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Tour business operations.
/// </summary>
public interface ITourService
{
    /// <summary>Gets all active tours asynchronously.</summary>
    Task<IEnumerable<TourDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets a tour by its identifier asynchronously.</summary>
    Task<TourDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Creates a new tour asynchronously.</summary>
    Task<TourDto> CreateAsync(TourCreateDto createDto, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing tour asynchronously.</summary>
    Task<TourDto?> UpdateAsync(int id, TourUpdateDto updateDto, CancellationToken cancellationToken = default);

    /// <summary>Deletes a tour by its identifier asynchronously.</summary>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Searches tours by name or place asynchronously.</summary>
    Task<IEnumerable<TourDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
