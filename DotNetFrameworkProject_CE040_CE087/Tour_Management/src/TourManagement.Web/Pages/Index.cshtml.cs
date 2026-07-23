using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages;

/// <summary>
/// Page model for the home/index page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets the recent tours to display on the home page.</summary>
    public IEnumerable<Tour> RecentTours { get; private set; } = Enumerable.Empty<Tour>();

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the home page.</summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var allTours = await _tourService.GetAllAsync(cancellationToken);
            RecentTours = allTours.Take(6);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading home page tours");
            RecentTours = Enumerable.Empty<Tour>();
        }
    }
}
