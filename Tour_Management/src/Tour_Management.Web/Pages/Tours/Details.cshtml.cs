using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>
/// Page model for the Tour details page.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(ITourService tourService, ILogger<DetailsModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    public TourViewModel? Tour { get; set; }
    public bool IsAdmin => HttpContext.Session.GetString("IsAdmin") == "true";

    public async Task OnGetAsync(int id)
    {
        try
        {
            var tour = await _tourService.GetTourByIdAsync(id);
            if (tour != null)
            {
                Tour = new TourViewModel
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
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour details for ID {TourId}", id);
        }
    }
}
