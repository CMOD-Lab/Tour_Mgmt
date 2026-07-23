using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for viewing tour details.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DetailsModel> _logger;

    /// <summary>Gets or sets the tour view model.</summary>
    public TourViewModel? Tour { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="DetailsModel"/>.
    /// </summary>
    public DetailsModel(ITourService tourService, ILogger<DetailsModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the tour details page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _tourService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
                return NotFound();

            Tour = new TourViewModel
            {
                Id = dto.Id,
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
            TempData["ErrorMessage"] = "An error occurred while loading tour details.";
            return RedirectToPage("Index");
        }
    }
}
