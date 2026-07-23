using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin.Tours;

/// <summary>
/// Page model for deleting a tour (admin).
/// </summary>
public class DeleteModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets or sets the tour to delete.</summary>
    public TourDeleteViewModel? Tour { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteModel"/> class.
    /// </summary>
    public DeleteModel(ITourService tourService, ILogger<DeleteModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the tour delete confirmation page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        var dto = await _tourService.GetByIdAsync(id, cancellationToken);
        if (dto == null)
            return NotFound();

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

    /// <summary>Handles POST requests for the tour deletion.</summary>
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        try
        {
            await _tourService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Admin deleted tour id {TourId}", id);
            TempData["Success"] = "Tour deleted successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour id {TourId}", id);
            TempData["Error"] = "An error occurred while deleting the tour.";
            return RedirectToPage("./Index");
        }
    }
}
