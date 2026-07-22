using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for the all bookings listing page (admin only).
/// </summary>
public class IndexModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets the list of all bookings.</summary>
    public IEnumerable<BookingDto> Bookings { get; private set; } = Enumerable.Empty<BookingDto>();

    /// <summary>
    /// Initializes a new instance of <see cref="IndexModel"/>.
    /// </summary>
    public IndexModel(IBookingService bookingService, ILogger<IndexModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the all bookings page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            TempData["ErrorMessage"] = "You must be an admin to view all bookings.";
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            Bookings = await _bookingService.GetAllAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading all bookings");
            TempData["ErrorMessage"] = "An error occurred while loading bookings.";
            return Page();
        }
    }
}
