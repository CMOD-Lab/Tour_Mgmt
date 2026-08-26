using Microsoft.Extensions.Logging;
using TourBooking.Domain.Entities;
using TourBooking.Domain.Exceptions;
using TourBooking.Domain.Interfaces.Repositories;
using TourBooking.Domain.Interfaces.Services;

namespace TourBooking.Application.Services;

/// <summary>
/// Service implementation for Tour business operations.
/// </summary>
public class TourService : ITourService
{
    private readonly ITourRepository _tourRepository;
    private readonly ILogger<TourService> _logger;

    /// <summary>Initializes a new instance of the <see cref="TourService"/> class.</summary>
    public TourService(ITourRepository tourRepository, ILogger<TourService> logger)
    {
        _tourRepository = tourRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Tour>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all tours");
            return await _tourRepository.GetAllAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all tours");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Tour?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving tour with ID: {TourId}", id);
            return await _tourRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tour with ID: {TourId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> CreateAsync(Tour tour, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new tour: {TourName}", tour.TourName);
            await _tourRepository.AddAsync(tour, cancellationToken);
            _logger.LogInformation("Tour created successfully: {TourName}", tour.TourName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", tour.TourName);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(Tour tour, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating tour with ID: {TourId}", tour.TourId);

            if (!await _tourRepository.ExistsAsync(tour.TourId, cancellationToken))
            {
                throw new NotFoundException(nameof(Tour), tour.TourId);
            }

            await _tourRepository.UpdateAsync(tour, cancellationToken);
            _logger.LogInformation("Tour updated successfully with ID: {TourId}", tour.TourId);
            return true;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with ID: {TourId}", tour.TourId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting tour with ID: {TourId}", id);

            if (!await _tourRepository.ExistsAsync(id, cancellationToken))
            {
                throw new NotFoundException(nameof(Tour), id);
            }

            await _tourRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Tour deleted successfully with ID: {TourId}", id);
            return true;
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID: {TourId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Tour>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching tours with term: {SearchTerm}", searchTerm);
            return await _tourRepository.SearchAsync(searchTerm, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching tours with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
