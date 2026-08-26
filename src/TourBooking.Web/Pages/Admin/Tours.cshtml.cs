using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Admin;

/// <summary>
/// Page model for the admin tours management page.
/// </summary>
public class ToursModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<ToursModel> _logger;

    /// <summary>Gets or sets the list of tours.</summary>
    public IEnumerable<TourViewModel> Tours { get; set; } = Enumerable.Empty<TourViewModel>();

    /// <summary>Initializes a new instance of the <see cref="ToursModel"/> class.</summary>
    public ToursModel(ITourService tourService, ILogger<ToursModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the admin tours page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var tours = await _tourService.GetAllAsync(cancellationToken);

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

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin tours");
            return Page();
        }
    }
}
