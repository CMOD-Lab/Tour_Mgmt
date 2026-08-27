using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for the Tour details page.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DetailsModel> _logger;

    /// <summary>Gets the tour details to display.</summary>
    public TourDetailsViewModel? Tour { get; private set; }

    /// <summary>Initializes a new instance of DetailsModel.</summary>
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
            {
                _logger.LogWarning("Tour with ID {TourId} not found", id);
                return NotFound();
            }

            // Manual mapping from DTO to ViewModel
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
                CreatedDate = dto.CreatedDate
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour details for ID {TourId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading tour details.";
            return RedirectToPage("Index");
        }
    }
}
