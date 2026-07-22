using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Application.Interfaces;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for Booking business operations.
/// </summary>
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BookingService> _logger;

    /// <summary>
    /// Initializes a new instance of <see cref="BookingService"/>.
    /// </summary>
    public BookingService(IBookingRepository bookingRepository, IMapper mapper, ILogger<BookingService> logger)
    {
        _bookingRepository = bookingRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BookingDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all bookings");
            var bookings = await _bookingRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all bookings");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<BookingDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving booking with id {BookingId}", id);
            var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);
            return booking == null ? null : _mapper.Map<BookingDto>(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving booking with id {BookingId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BookingDto>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving bookings for email: {Email}", email);
            var bookings = await _bookingRepository.GetByEmailAsync(email, cancellationToken);
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookings for email: {Email}", email);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<BookingDto> CreateAsync(BookingCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new booking for tour: {TourName}", dto.TourName);
            var booking = _mapper.Map<Booking>(dto);
            var created = await _bookingRepository.AddAsync(booking, cancellationToken);
            _logger.LogInformation("Booking created successfully with id {BookingId}", created.Id);
            return _mapper.Map<BookingDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking for tour: {TourName}", dto.TourName);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(int id, BookingUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating booking with id {BookingId}", id);
            var existing = await _bookingRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(Booking), id);

            _mapper.Map(dto, existing);
            await _bookingRepository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Booking updated successfully with id {BookingId}", id);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking with id {BookingId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting booking with id {BookingId}", id);
            var exists = await _bookingRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
                throw new NotFoundException(nameof(Booking), id);

            await _bookingRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Booking deleted successfully with id {BookingId}", id);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with id {BookingId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<BookingDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching bookings with term: {SearchTerm}", searchTerm);
            var bookings = await _bookingRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching bookings with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
