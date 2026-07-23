using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>
/// Page model for the tour details page.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly TourService _tourService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(TourService tourService, ILogger<DetailsModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    public TourDetailsViewModel? Tour { get; set; }

    public async Task OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _tourService.GetByIdAsync(id, cancellationToken);
            if (dto != null)
            {
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
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tour details for ID {TourId}", id);
        }
    }
}
