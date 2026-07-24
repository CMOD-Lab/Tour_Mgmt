using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>Page model for the tours list page.</summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets or sets the list of tours to display.</summary>
    public IEnumerable<TourViewModel> Tours { get; set; } = Enumerable.Empty<TourViewModel>();

    /// <summary>Gets or sets the current search term.</summary>
    public string SearchTerm { get; set; } = string.Empty;

    /// <summary>Initializes a new instance of <see cref="IndexModel"/>.</summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the tours list page.</summary>
    public async Task OnGetAsync(string? search, CancellationToken cancellationToken)
    {
        try
        {
            SearchTerm = search ?? string.Empty;

            var tours = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _tourService.GetAllAsync(cancellationToken)
                : await _tourService.SearchAsync(SearchTerm, cancellationToken);

            // Manual mapping from DTO to ViewModel
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
            _logger.LogError(ex, "Error loading tours list");
        }
    }
}
