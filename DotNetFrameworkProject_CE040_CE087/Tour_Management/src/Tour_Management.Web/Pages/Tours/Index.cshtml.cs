using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>
/// Page model for the tours index/list page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Gets the list of tours to display.</summary>
    public IEnumerable<TourIndexViewModel> Tours { get; private set; } = Enumerable.Empty<TourIndexViewModel>();

    /// <summary>Gets or sets the search term.</summary>
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    /// <summary>Gets or sets the success message.</summary>
    public string? Message { get; set; }

    /// <summary>
    /// Handles GET requests for the tours list page.
    /// </summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Loading tours list");
            Message = TempData["Message"]?.ToString();

            IEnumerable<Domain.Entities.Tour> tours;
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                tours = await _tourService.SearchAsync(SearchTerm, cancellationToken);
            }
            else
            {
                tours = await _tourService.GetAllAsync(cancellationToken);
            }

            // Manual ViewModel mapping (Web layer)
            Tours = tours.Select(t => new TourIndexViewModel
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
            _logger.LogError(ex, "Error loading tours list");
            ModelState.AddModelError(string.Empty, "An error occurred while loading tours.");
        }
    }
}
