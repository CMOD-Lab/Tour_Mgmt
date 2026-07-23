using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for the user's bookings page.
/// </summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    public IEnumerable<BookingDto> Bookings { get; private set; } = Enumerable.Empty<BookingDto>();
    public string? SuccessMessage { get; private set; }

    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string? message = null, CancellationToken cancellationToken = default)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Account/Login");
        }

        if (!string.IsNullOrEmpty(message))
        {
            SuccessMessage = message;
        }

        try
        {
            Bookings = await _bookingService.GetByUserEmailAsync(email, cancellationToken);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings for user {Email}.", email);
            Bookings = Enumerable.Empty<BookingDto>();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Account/Login");
        }

        try
        {
            await _bookingService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Booking {BookingId} cancelled by user {Email}.", id, email);
            return RedirectToPage(new { message = "Booking cancelled successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking {BookingId}.", id);
            return RedirectToPage();
        }
    }
}
