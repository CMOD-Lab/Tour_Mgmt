using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>
/// Page model for the tour details page.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DetailsModel> _logger;

    /// <summary>Gets or sets the tour details to display.</summary>
    public TourDetailsViewModel? Tour { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DetailsModel"/> class.
    /// </summary>
    public DetailsModel(ITourService tourService, ILogger<DetailsModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the tour details page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _tourService.GetByIdAsync(id, cancellationToken);
            if (dto == null)
                return NotFound();

            Tour = new TourDetailsViewModel
            {
                TourId = dto.TourId,
                TourName = dto.TourName,
                Place = dto.Place,
                Days = dto.Days,
                Price = dto.Price,
                Locations = dto.Locations,
                TourInfo = dto.TourInfo,
                Pic = dto.Pic,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour details for id {TourId}", id);
            TempData["Error"] = "An error occurred while loading tour details.";
            return RedirectToPage("./Index");
        }
    }
}
