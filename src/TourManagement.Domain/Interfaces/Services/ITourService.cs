using TourManagement.Domain.Entities;

namespace TourManagement.Domain.Interfaces.Services;

/// <summary>
/// Service interface for Tour business operations.
/// </summary>
public interface ITourService
{
    Task<IEnumerable<Tour>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Tour?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CreateAsync(Tour tour, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Tour tour, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Tour>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
