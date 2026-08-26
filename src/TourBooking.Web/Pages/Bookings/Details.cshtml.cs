using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Bookings;

/// <summary>
/// Page model for the booking details page.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DetailsModel> _logger;

    /// <summary>Gets or sets the booking view model.</summary>
    public BookingViewModel? Booking { get; set; }

    /// <summary>Initializes a new instance of the <see cref="DetailsModel"/> class.</summary>
    public DetailsModel(IBookingService bookingService, ILogger<DetailsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the booking details page.</summary>
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

            // Manually map domain entity to ViewModel
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
            _logger.LogError(ex, "Error loading booking details for ID: {BookingId}", id);
            return RedirectToPage("/Bookings/MyBookings");
        }
    }
}
