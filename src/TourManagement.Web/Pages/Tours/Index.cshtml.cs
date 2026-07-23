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

    public IEnumerable<TourDto> Tours { get; private set; } = Enumerable.Empty<TourDto>();
    public string? SearchTerm { get; private set; }

    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    public async Task OnGetAsync(string? search = null, CancellationToken cancellationToken = default)
    {
        SearchTerm = search;
        try
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                Tours = await _tourService.SearchAsync(search, cancellationToken);
            }
            else
            {
                Tours = await _tourService.GetActiveToursAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours list.");
            Tours = Enumerable.Empty<TourDto>();
        }
    }
}
