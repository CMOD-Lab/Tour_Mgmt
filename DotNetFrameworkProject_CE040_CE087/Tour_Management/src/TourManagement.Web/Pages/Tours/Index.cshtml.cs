using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for the tours index/list page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets or sets the list of tours to display.</summary>
    public IEnumerable<TourViewModel> Tours { get; set; } = Enumerable.Empty<TourViewModel>();

    /// <summary>Gets or sets the search term.</summary>
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="IndexModel"/>.
    /// </summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the tours list page.
    /// </summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            IEnumerable<TourDto> tours;
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                tours = await _tourService.SearchAsync(SearchTerm, cancellationToken);
            }
            else
            {
                tours = await _tourService.GetAllAsync(cancellationToken);
            }

            Tours = tours.Select(MapToViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours list");
            TempData["ErrorMessage"] = "An error occurred while loading tours.";
        }
    }

    private static TourViewModel MapToViewModel(TourDto dto) => new()
    {
        Id = dto.Id,
        TourName = dto.TourName,
        Place = dto.Place,
        Days = dto.Days,
        Price = dto.Price,
        Locations = dto.Locations,
        TourInfo = dto.TourInfo,
        Pic = dto.Pic,
        CreatedDate = dto.CreatedDate,
        IsActive = dto.IsActive
    };
}
