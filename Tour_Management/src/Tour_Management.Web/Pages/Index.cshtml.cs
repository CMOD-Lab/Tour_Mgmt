using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages;

/// <summary>
/// Page model for the home/index page.
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

    public IEnumerable<TourViewModel> FeaturedTours { get; set; } = Enumerable.Empty<TourViewModel>();
    public string? UserEmail => HttpContext.Session.GetString("UserEmail");

    public async Task OnGetAsync()
    {
        try
        {
            var tours = await _tourService.GetAllToursAsync();
            FeaturedTours = tours.Take(6).Select(MapToViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading featured tours on home page");
            FeaturedTours = Enumerable.Empty<TourViewModel>();
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
