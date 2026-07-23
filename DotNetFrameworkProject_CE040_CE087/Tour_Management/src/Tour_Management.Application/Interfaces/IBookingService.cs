using Tour_Management.Application.DTOs;

namespace Tour_Management.Application.Interfaces;

/// <summary>
/// Service interface for Booking business operations.
/// </summary>
public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<BookingDto> CreateAsync(BookingCreateDto createDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, BookingUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BookingDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}
