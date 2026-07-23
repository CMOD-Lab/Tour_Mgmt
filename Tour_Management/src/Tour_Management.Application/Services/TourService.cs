using AutoMapper;
using Microsoft.Extensions.Logging;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Domain.Interfaces.Repositories;
using Tour_Management.Domain.Interfaces.Services;

namespace Tour_Management.Application.Services;

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
            _logger.LogInformation("Creating new tour: {TourName}", tour.TourName);
            tour.CreatedDate = DateTime.UtcNow;
            tour.IsActive = true;
            var created = await _tourRepository.AddAsync(tour, cancellationToken);
            _logger.LogInformation("Tour created successfully with ID {TourId}", created.TourId);
            return created;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", tour.TourName);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<Tour> UpdateTourAsync(Tour tour, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating tour with ID {TourId}", tour.TourId);
            var existing = await _tourRepository.GetByIdAsync(tour.TourId, cancellationToken)
                ?? throw new NotFoundException(nameof(Tour), tour.TourId);
            tour.CreatedDate = existing.CreatedDate;
            tour.IsActive = existing.IsActive;
            tour.ModifiedDate = DateTime.UtcNow;
            var updated = await _tourRepository.UpdateAsync(tour, cancellationToken);
            _logger.LogInformation("Tour updated successfully with ID {TourId}", updated.TourId);
            return updated;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with ID {TourId}", tour.TourId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteTourAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting tour with ID {TourId}", id);
            if (!await _tourRepository.ExistsAsync(id, cancellationToken))
                throw new NotFoundException(nameof(Tour), id);
            await _tourRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Tour deleted successfully with ID {TourId}", id);
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
