using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin.Tours;

/// <summary>
/// Page model for the admin tours listing page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets or sets the list of tours.</summary>
    public IEnumerable<TourIndexViewModel> Tours { get; set; } = Enumerable.Empty<TourIndexViewModel>();

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the admin tours listing page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        try
        {
            var dtos = await _tourService.GetAllAsync(cancellationToken);
            Tours = dtos.Select(t => new TourIndexViewModel
            {
                TourId = t.TourId,
                TourName = t.TourName,
                Place = t.Place,
                Days = t.Days,
                Price = t.Price,
                Locations = t.Locations,
                Pic = t.Pic,
                IsActive = t.IsActive
            });
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin tours list");
            TempData["Error"] = "An error occurred while loading tours.";
            return Page();
        }
    }
}
