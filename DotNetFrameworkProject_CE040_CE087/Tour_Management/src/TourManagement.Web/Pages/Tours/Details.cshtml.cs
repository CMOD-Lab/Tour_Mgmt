using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Tours;

/// <summary>
/// Page model for the tour details page.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<DetailsModel> _logger;

    /// <summary>Gets the tour details.</summary>
    public TourDto? Tour { get; private set; }

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
    public async Task OnGetAsync(int id)
    {
        try
        {
            Tour = await _tourService.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour details for id {TourId}", id);
        }
    }
}
