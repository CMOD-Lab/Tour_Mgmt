using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for the tours listing page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets the list of tours to display.</summary>
    public IEnumerable<TourDto> Tours { get; private set; } = Enumerable.Empty<TourDto>();

    /// <summary>Gets the current search term.</summary>
    public string? SearchTerm { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="IndexModel"/>.
    /// </summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the tours listing page.
    /// </summary>
    public async Task OnGetAsync(string? searchTerm = null)
    {
        try
        {
            SearchTerm = searchTerm;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                Tours = await _tourService.SearchAsync(searchTerm);
            }
            else
            {
                Tours = await _tourService.GetAllAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours list");
            TempData["ErrorMessage"] = "An error occurred while loading tours.";
        }
    }
}
