using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>
/// Page model for the tours listing page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets or sets the list of tours to display.</summary>
    public IEnumerable<TourIndexViewModel> Tours { get; set; } = Enumerable.Empty<TourIndexViewModel>();

    /// <summary>Gets or sets the current search term.</summary>
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the tours listing page.</summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var dtos = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _tourService.GetAllAsync(cancellationToken)
                : await _tourService.SearchAsync(SearchTerm, cancellationToken);

            Tours = dtos.Select(t => new TourIndexViewModel
            {
                TourId = t.TourId,
                TourName = t.TourName,
                Place = t.Place,
                Days = t.Days,
                Price = t.Price,
                Locations = t.Locations,
                Pic = t.Pic,
                IsActive = t.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours");
            TempData["Error"] = "An error occurred while loading tours.";
        }
    }
}
