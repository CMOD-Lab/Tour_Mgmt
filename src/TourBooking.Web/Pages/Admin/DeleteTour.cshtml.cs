using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Admin;

/// <summary>
/// Page model for the admin delete tour page.
/// </summary>
public class DeleteTourModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DeleteTourModel> _logger;

    /// <summary>Gets or sets the tour view model.</summary>
    public TourViewModel? Tour { get; set; }

    /// <summary>Initializes a new instance of the <see cref="DeleteTourModel"/> class.</summary>
    public DeleteTourModel(ITourService tourService, ILogger<DeleteTourModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the delete tour page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var tour = await _tourService.GetByIdAsync(id, cancellationToken);
            if (tour == null)
            {
                return NotFound();
            }

            Tour = new TourViewModel
            {
                TourId = tour.TourId,
                TourName = tour.TourName,
                Place = tour.Place,
                Days = tour.Days,
                Price = tour.Price,
                Locations = tour.Locations,
                TourInfo = tour.TourInfo,
                Pic = tour.Pic
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for deletion, ID: {TourId}", id);
            return RedirectToPage("/Admin/Tours");
        }
    }

    /// <summary>Handles POST requests for the delete tour form submission.</summary>
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            await _tourService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Tour deleted with ID: {TourId}", id);
            return RedirectToPage("/Admin/Tours");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID: {TourId}", id);
            return RedirectToPage("/Admin/Tours");
        }
    }
}
