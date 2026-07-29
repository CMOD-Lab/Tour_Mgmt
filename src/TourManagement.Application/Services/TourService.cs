using AutoMapper;
using Microsoft.Extensions.Logging;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Repositories;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Application.Services;

/// <summary>
/// Service implementation for Tour business operations.
/// </summary>
public class TourService : ITourService
{
    private readonly ITourRepository _tourRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TourService> _logger;

    public TourService(ITourRepository tourRepository, IMapper mapper, ILogger<TourService> logger)
    {
        _tourRepository = tourRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Tour>> GetAllToursAsync(CancellationToken cancellationToken = default)
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
    public async Task<Tour?> GetTourByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving tour with ID {TourId}", id);
            return await _tourRepository.GetByIdAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tour with ID {TourId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Tour> CreateTourAsync(Tour tour, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(tour);
            _logger.LogInformation("Creating new tour: {TourName}", tour.TourName);
            tour.CreatedDate = DateTime.UtcNow;
            tour.IsActive = true;
            return await _tourRepository.AddAsync(tour, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", tour?.TourName);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Tour> UpdateTourAsync(Tour tour, CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(tour);
            var existing = await _tourRepository.GetByIdAsync(tour.TourId, cancellationToken)
                ?? throw new NotFoundException(nameof(Tour), tour.TourId);
            _logger.LogInformation("Updating tour with ID {TourId}", tour.TourId);
            tour.ModifiedDate = DateTime.UtcNow;
            return await _tourRepository.UpdateAsync(tour, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with ID {TourId}", tour?.TourId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteTourAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var exists = await _tourRepository.ExistsAsync(id, cancellationToken);
            if (!exists)
                throw new NotFoundException(nameof(Tour), id);
            _logger.LogInformation("Deleting tour with ID {TourId}", id);
            await _tourRepository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID {TourId}", id);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Tour>> SearchToursAsync(string searchTerm, CancellationToken cancellationToken = default)
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
