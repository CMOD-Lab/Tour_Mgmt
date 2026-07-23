using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for tours list/index page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    public IEnumerable<TourListViewModel> Tours { get; set; } = Enumerable.Empty<TourListViewModel>();
    public string SearchTerm { get; set; } = string.Empty;

    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    public async Task OnGetAsync(string? searchTerm, CancellationToken cancellationToken)
    {
        SearchTerm = searchTerm ?? string.Empty;

        try
        {
            var tours = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _tourService.GetAllAsync(cancellationToken)
                : await _tourService.SearchAsync(SearchTerm, cancellationToken);

            // Manually map domain entities to ViewModels
            Tours = tours.Select(t => new TourListViewModel
            {
                TourId = t.TourId,
                TourName = t.TourName,
                Place = t.Place,
                Days = t.Days,
                Price = t.Price,
                Locations = t.Locations,
                Pic = t.Pic
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours list");
        }
    }
}
