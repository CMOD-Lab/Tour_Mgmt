using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for listing all tours.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    public IEnumerable<TourViewModel> Tours { get; set; } = Enumerable.Empty<TourViewModel>();
    public string? SearchTerm { get; set; }
    public bool IsUserLoggedIn { get; set; }
    public bool IsAdmin { get; set; }

    public async Task OnGetAsync(string? searchTerm, CancellationToken cancellationToken)
    {
        try
        {
            SearchTerm = searchTerm;
            IsUserLoggedIn = HttpContext.Session.GetString("UserEmail") != null;
            IsAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

            var tours = string.IsNullOrWhiteSpace(searchTerm)
                ? await _tourService.GetAllToursAsync(cancellationToken)
                : await _tourService.SearchToursAsync(searchTerm, cancellationToken);

            Tours = tours.Select(t => new TourViewModel
            {
                TourId = t.TourId,
                TourName = t.TourName,
                Place = t.Place,
                Days = t.Days,
                Price = t.Price,
                Locations = t.Locations,
                TourInfo = t.TourInfo,
                Pic = t.Pic,
                CreatedDate = t.CreatedDate,
                IsActive = t.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours page");
            Tours = Enumerable.Empty<TourViewModel>();
        }
    }
}
