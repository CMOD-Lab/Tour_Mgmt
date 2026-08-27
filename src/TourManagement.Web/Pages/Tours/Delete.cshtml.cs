using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for deleting a tour.
/// </summary>
[Authorize(Roles = "Admin")]
public class DeleteModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets or sets the tour delete view model.</summary>
    [BindProperty]
    public TourDeleteViewModel? Tour { get; set; }

    /// <summary>Initializes a new instance of DeleteModel.</summary>
    public DeleteModel(ITourService tourService, ILogger<DeleteModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the delete tour confirmation page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _tourService.GetByIdAsync(id, cancellationToken);
            if (dto == null)
                return NotFound();

            // Manual mapping from DTO to ViewModel
            Tour = new TourDeleteViewModel
            {
                TourId = dto.TourId,
                TourName = dto.TourName,
                Place = dto.Place,
                Days = dto.Days,
                Price = dto.Price
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for delete, ID {TourId}", id);
            return RedirectToPage("Index");
        }
    }

    /// <summary>Handles POST requests to delete a tour.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (Tour == null)
            return RedirectToPage("Index");

        try
        {
            await _tourService.DeleteAsync(Tour.TourId, cancellationToken);
            TempData["SuccessMessage"] = "Tour deleted successfully!";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID {TourId}", Tour.TourId);
            TempData["ErrorMessage"] = "An error occurred while deleting the tour.";
            return RedirectToPage("Index");
        }
    }
}
