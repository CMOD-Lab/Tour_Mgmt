using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Tours;

/// <summary>
/// Page model for the tours listing page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets or sets the list of tours.</summary>
    public IEnumerable<TourViewModel> Tours { get; set; } = Enumerable.Empty<TourViewModel>();

    /// <summary>Gets or sets the search term.</summary>
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    /// <summary>Initializes a new instance of the <see cref="IndexModel"/> class.</summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the tours listing page.</summary>
    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var tours = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _tourService.GetAllAsync(cancellationToken)
                : await _tourService.SearchAsync(SearchTerm, cancellationToken);

            // Manually map domain entities to ViewModels
            Tours = tours.Select(t => new TourViewModel
            {
                TourId = t.TourId,
                TourName = t.TourName,
                Place = t.Place,
                Days = t.Days,
                Price = t.Price,
                Locations = t.Locations,
                TourInfo = t.TourInfo,
                Pic = t.Pic
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours");
        }
    }
}
