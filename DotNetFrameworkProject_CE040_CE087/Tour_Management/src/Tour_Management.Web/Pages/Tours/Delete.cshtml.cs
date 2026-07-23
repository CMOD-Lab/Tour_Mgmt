using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Services;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>
/// Page model for deleting a tour.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly TourService _tourService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(TourService tourService, ILogger<DeleteModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    [BindProperty]
    public TourDeleteViewModel? Tour { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

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
                Price = dto.Price
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour for delete, ID {TourId}", id);
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        if (Tour == null)
            return RedirectToPage("./Index");

        try
        {
            await _tourService.DeleteAsync(Tour.TourId, cancellationToken);
            TempData["SuccessMessage"] = $"Tour '{Tour.TourName}' deleted successfully!";
            return RedirectToPage("./Index");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID {TourId}", Tour?.TourId);
            TempData["ErrorMessage"] = "An error occurred while deleting the tour.";
            return RedirectToPage("./Index");
        }
    }
}
