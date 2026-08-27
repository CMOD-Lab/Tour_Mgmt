using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for the Tours index/list page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets the list of tours to display.</summary>
    public IEnumerable<TourListViewModel> Tours { get; private set; } = Enumerable.Empty<TourListViewModel>();

    /// <summary>Gets or sets the search term.</summary>
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    /// <summary>Initializes a new instance of IndexModel.</summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the tours list page.</summary>
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

            // Manual mapping from DTO to ViewModel (Web layer responsibility)
            Tours = tours.Select(t => new TourListViewModel
            {
                TourId = t.TourId,
                TourName = t.TourName,
                Place = t.Place,
                Days = t.Days,
                Price = t.Price,
                Locations = t.Locations,
                Pic = t.Pic
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours list");
            TempData["ErrorMessage"] = "An error occurred while loading tours.";
        }
    }
}
