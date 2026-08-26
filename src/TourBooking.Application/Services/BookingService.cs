using Microsoft.Extensions.Logging;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Exceptions;
using TourBooking.Domain.Interfaces.Repositories;
using TourBooking.Domain.Interfaces.Services;

namespace TourBooking.Application.Services;

/// <summary>
/// Service implementation for Booking business operations.
/// </summary>
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<BookingService> _logger;

    /// <summary>Initializes a new instance of the <see cref="BookingService"/> class.</summary>
    public BookingService(IBookingRepository bookingRepository, ILogger<BookingService> logger)
    {
        _bookingRepository = bookingRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken cancellationToken = default)
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
    public async Task<Booking?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving booking with ID: {BookingId}", id);
            return await _bookingRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving booking with ID: {BookingId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bookings for email: {Email}", email);
            return await _bookingRepository.GetByEmailAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookings for email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> CreateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new booking for tour: {TourName}", booking.TourName);
            await _bookingRepository.AddAsync(booking, cancellationToken);
            _logger.LogInformation("Booking created successfully for tour: {TourName}", booking.TourName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", booking.TourName);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating booking with ID: {BookingId}", booking.TourId);

            if (!await _bookingRepository.ExistsAsync(booking.TourId, cancellationToken))
            {
                throw new NotFoundException(nameof(Booking), booking.TourId);
            }

            await _bookingRepository.UpdateAsync(booking, cancellationToken);
            _logger.LogInformation("Booking updated successfully with ID: {BookingId}", booking.TourId);
            return true;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking with ID: {BookingId}", booking.TourId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting booking with ID: {BookingId}", id);

            if (!await _bookingRepository.ExistsAsync(id, cancellationToken))
            {
                throw new NotFoundException(nameof(Booking), id);
            }

            await _bookingRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Booking deleted successfully with ID: {BookingId}", id);
            return true;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with ID: {BookingId}", id);
            throw;
        }
    }
}
