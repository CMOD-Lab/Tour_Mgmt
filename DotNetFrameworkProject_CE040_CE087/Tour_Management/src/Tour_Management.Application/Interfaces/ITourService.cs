using Tour_Management.Application.DTOs;

namespace Tour_Management.Application.Interfaces;

/// <summary>
/// Service interface for Tour business operations.
/// </summary>
public interface ITourService
{
    Task<IEnumerable<TourDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TourDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TourDto> CreateAsync(TourCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, TourUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TourDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
