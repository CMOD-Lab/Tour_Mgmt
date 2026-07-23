using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Tours;

/// <summary>Page model for the tours index/list page.</summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    public IEnumerable<TourViewModel> Tours { get; set; } = Enumerable.Empty<TourViewModel>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var dtos = string.IsNullOrWhiteSpace(SearchTerm)
                ? await _tourService.GetAllAsync(cancellationToken)
                : await _tourService.SearchAsync(SearchTerm, cancellationToken);

            // Manual mapping from DTO to ViewModel (Web layer responsibility)
            Tours = dtos.Select(dto => new TourViewModel
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
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading tours index page");
            Tours = Enumerable.Empty<TourViewModel>();
        }
    }
}
