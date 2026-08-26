using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Bookings;

/// <summary>
/// Page model for the delete/cancel booking page.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets or sets the booking view model.</summary>
    public BookingViewModel? Booking { get; set; }

    /// <summary>Initializes a new instance of the <see cref="DeleteModel"/> class.</summary>
    public DeleteModel(IBookingService bookingService, ILogger<DeleteModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the delete booking page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            var booking = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (booking == null)
            {
                return NotFound();
            }

            Booking = new BookingViewModel
            {
                TourId = booking.TourId,
                TourName = booking.TourName,
                Place = booking.Place,
                Email = booking.Email,
                FirstName = booking.FirstName
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for deletion, ID: {BookingId}", id);
            return RedirectToPage("/Bookings/MyBookings");
        }
    }

    /// <summary>Handles POST requests for the delete booking form submission.</summary>
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _bookingService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Booking deleted with ID: {BookingId}", id);
            return RedirectToPage("/Bookings/MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with ID: {BookingId}", id);
            return RedirectToPage("/Bookings/MyBookings");
        }
    }
}
