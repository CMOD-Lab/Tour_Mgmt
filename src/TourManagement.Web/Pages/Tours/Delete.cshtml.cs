using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for deleting a tour.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(ITourService tourService, ILogger<DeleteModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    [BindProperty]
    public TourViewModel? Tour { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        var tour = await _tourService.GetTourByIdAsync(id, cancellationToken);
        if (tour == null)
            return NotFound();

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

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        if (Tour == null)
            return NotFound();

        try
        {
            await _tourService.DeleteTourAsync(Tour.TourId, cancellationToken);
            TempData["SuccessMessage"] = "Tour deleted successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID {TourId}", Tour.TourId);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the tour.");
            return Page();
        }
    }
}
