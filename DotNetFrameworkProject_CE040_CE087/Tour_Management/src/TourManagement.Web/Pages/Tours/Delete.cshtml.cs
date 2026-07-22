using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for deleting a tour.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets the tour to be deleted.</summary>
    public TourDto? Tour { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteModel"/>.
    /// </summary>
    public DeleteModel(ITourService tourService, ILogger<DeleteModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the delete tour confirmation page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            TempData["ErrorMessage"] = "You must be an admin to delete tours.";
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            Tour = await _tourService.GetByIdAsync(id);
            if (Tour == null)
            {
                TempData["ErrorMessage"] = "Tour not found.";
                return RedirectToPage("Index");
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for delete, id {TourId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the tour.";
            return RedirectToPage("Index");
        }
    }

    /// <summary>
    /// Handles POST requests to delete a tour.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var tour = await _tourService.GetByIdAsync(id);
            var tourName = tour?.TourName ?? "Tour";

            await _tourService.DeleteAsync(id);
            TempData["SuccessMessage"] = $"Tour '{tourName}' was deleted successfully.";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "Tour not found.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with id {TourId}", id);
            TempData["ErrorMessage"] = "An error occurred while deleting the tour.";
            return RedirectToPage("Index");
        }
    }
}
