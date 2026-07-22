using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for the user's own bookings page.
/// </summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    /// <summary>Gets the user's bookings.</summary>
    public IEnumerable<BookingDto> Bookings { get; private set; } = Enumerable.Empty<BookingDto>();

    /// <summary>
    /// Initializes a new instance of <see cref="MyBookingsModel"/>.
    /// </summary>
    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the my bookings page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync()
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            TempData["ErrorMessage"] = "Please login to view your bookings.";
            return RedirectToPage("/Users/Login");
        }

        try
        {
            Bookings = await _bookingService.GetByEmailAsync(email);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings for {Email}", email);
            TempData["ErrorMessage"] = "An error occurred while loading your bookings.";
            return Page();
        }
    }
}
