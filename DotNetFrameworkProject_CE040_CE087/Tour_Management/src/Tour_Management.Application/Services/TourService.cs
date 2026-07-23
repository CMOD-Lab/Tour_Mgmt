using AutoMapper;
using Microsoft.Extensions.Logging;
using Tour_Management.Application.DTOs;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Domain.Interfaces.Repositories;

namespace Tour_Management.Application.Services;

/// <summary>
/// Service for managing tour operations.
/// </summary>
public class TourService
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

    /// <summary>Gets all active tours.</summary>
    public async Task<IEnumerable<TourDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving all tours");
            var tours = await _tourRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<TourDto>>(tours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all tours");
            throw;
        }
    }

    /// <summary>Gets a tour by its identifier.</summary>
    public async Task<TourDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Retrieving tour with ID {TourId}", id);
            var tour = await _tourRepository.GetByIdAsync(id, cancellationToken);
            return tour == null ? null : _mapper.Map<TourDto>(tour);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tour with ID {TourId}", id);
            throw;
        }
    }

    /// <summary>Creates a new tour.</summary>
    public async Task<TourDto> CreateAsync(TourCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new tour: {TourName}", dto.TourName);
            var tour = _mapper.Map<Tour>(dto);
            var created = await _tourRepository.AddAsync(tour, cancellationToken);
            _logger.LogInformation("Tour created with ID {TourId}", created.TourId);
            return _mapper.Map<TourDto>(created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour: {TourName}", dto.TourName);
            throw;
        }
    }

    /// <summary>Updates an existing tour.</summary>
    public async Task UpdateAsync(int id, TourUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating tour with ID {TourId}", id);
            var existing = await _tourRepository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundException(nameof(Tour), id);
            _mapper.Map(dto, existing);
            await _tourRepository.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Tour with ID {TourId} updated successfully", id);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour with ID {TourId}", id);
            throw;
        }
    }

    /// <summary>Deletes a tour by its identifier.</summary>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting tour with ID {TourId}", id);
            if (!await _tourRepository.ExistsAsync(id, cancellationToken))
                throw new NotFoundException(nameof(Tour), id);
            await _tourRepository.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Tour with ID {TourId} deleted successfully", id);
        }
        catch (NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID {TourId}", id);
            throw;
        }
    }

    /// <summary>Searches tours by name or place.</summary>
    public async Task<IEnumerable<TourDto>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Searching tours with term: {SearchTerm}", searchTerm);
            var tours = await _tourRepository.SearchAsync(searchTerm, cancellationToken);
            return _mapper.Map<IEnumerable<TourDto>>(tours);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching tours with term: {SearchTerm}", searchTerm);
            throw;
        }
    }
}
