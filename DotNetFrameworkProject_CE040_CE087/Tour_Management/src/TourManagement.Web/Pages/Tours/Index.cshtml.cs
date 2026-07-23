using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for the tours index page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets the list of tours to display.</summary>
    public IEnumerable<Tour> Tours { get; private set; } = Enumerable.Empty<Tour>();

    /// <summary>Gets or sets the search term.</summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the tours index page.</summary>
    public async Task OnGetAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        try
        {
            SearchTerm = searchTerm;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                Tours = await _tourService.SearchAsync(searchTerm, cancellationToken);
            }
            else
            {
                Tours = await _tourService.GetAllAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours index page");
            Tours = Enumerable.Empty<Tour>();
        }
    }
}
