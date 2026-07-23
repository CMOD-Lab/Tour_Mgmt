using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Admin.Tours;

/// <summary>
/// Page model for the admin tours management page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly ILogger<IndexModel> _logger;

    public IEnumerable<TourDto> Tours { get; private set; } = Enumerable.Empty<TourDto>();
    public string? SuccessMessage { get; private set; }

    public IndexModel(ITourService tourService, ILogger<IndexModel> logger)
    {
        _tourService = tourService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string? message = null, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Account/AdminLogin");
        }

        if (!string.IsNullOrEmpty(message))
        {
            SuccessMessage = message;
        }

        try
        {
            Tours = await _tourService.GetAllAsync(cancellationToken);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin tours list.");
            Tours = Enumerable.Empty<TourDto>();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Account/AdminLogin");
        }

        try
        {
            await _tourService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Tour {TourId} deleted by admin.", id);
            return RedirectToPage(new { message = "Tour deleted successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour {TourId}.", id);
            return RedirectToPage();
        }
    }
}
