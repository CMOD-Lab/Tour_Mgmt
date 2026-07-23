using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>
/// Page model for the Tours index page.
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
    public bool IsAdmin => HttpContext.Session.GetString("IsAdmin") == "true";

    public async Task OnGetAsync(string? searchTerm = null)
    {
        try
        {
            SearchTerm = searchTerm;
            IEnumerable<Tour> tours;

            if (!string.IsNullOrWhiteSpace(searchTerm))
                tours = await _tourService.SearchToursAsync(searchTerm);
            else
                tours = await _tourService.GetAllToursAsync();

            Tours = tours.Select(MapToViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours list");
            Tours = Enumerable.Empty<TourViewModel>();
        }
    }

    private static TourViewModel MapToViewModel(Tour tour) => new()
    {
        TourId = tour.TourId,
        TourName = tour.TourName,
        Place = tour.Place,
        Days = tour.Days,
        Price = tour.Price,
        Locations = tour.Locations,
        TourInfo = tour.TourInfo,
        PicturePath = tour.PicturePath,
        CreatedDate = tour.CreatedDate
    };
}
