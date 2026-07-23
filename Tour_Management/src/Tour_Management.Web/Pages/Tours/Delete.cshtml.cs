using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

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

    public TourViewModel? Tour { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        try
        {
            var tour = await _tourService.GetTourByIdAsync(id);
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
                PicturePath = tour.PicturePath
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for delete, ID {TourId}", id);
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        try
        {
            await _tourService.DeleteTourAsync(id);
            TempData["SuccessMessage"] = "Tour deleted successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID {TourId}", id);
            TempData["ErrorMessage"] = "An error occurred while deleting the tour.";
            return RedirectToPage("Index");
        }
    }
}
