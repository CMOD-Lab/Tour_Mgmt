using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for Booking business operations.
/// </summary>
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BookingService> _logger;

    public BookingService(IBookingRepository bookingRepository, IMapper mapper, ILogger<BookingService> logger)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetAllBookingsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all bookings");
            return await _bookingRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all bookings");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Booking?> GetBookingByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving booking with ID {BookingId}", id);
            return await _bookingRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving booking with ID {BookingId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetBookingsByUserEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bookings for user: {Email}", email);
            return await _bookingRepository.GetByUserEmailAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookings for user: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Booking> CreateBookingAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(booking);
            _logger.LogInformation("Creating booking for user: {Email}", booking.Email);
            booking.CreatedDate = DateTime.UtcNow;
            booking.IsActive = true;
            return await _bookingRepository.AddAsync(booking, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for user: {Email}", booking?.Email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Booking> UpdateBookingAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(booking);
            var existing = await _bookingRepository.GetByIdAsync(booking.BookingId, cancellationToken)
                ?? throw new NotFoundException(nameof(Booking), booking.BookingId);
            _logger.LogInformation("Updating booking with ID {BookingId}", booking.BookingId);
            return await _bookingRepository.UpdateAsync(booking, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking with ID {BookingId}", booking?.BookingId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteBookingAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _bookingRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
                throw new NotFoundException(nameof(Booking), id);
            _logger.LogInformation("Deleting booking with ID {BookingId}", id);
            await _bookingRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with ID {BookingId}", id);
            throw;
        }
    }
}
