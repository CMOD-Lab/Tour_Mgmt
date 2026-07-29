using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for displaying booking details.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IBookingService bookingService, ILogger<DetailsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public BookingViewModel? Booking { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(id, cancellationToken);
            if (booking == null)
                return NotFound();

            // Users can only see their own bookings unless admin
            if (booking.Email != email && HttpContext.Session.GetString("IsAdmin") != "true")
                return Forbid();

            Booking = new BookingViewModel
            {
                BookingId = booking.BookingId,
                TourName = booking.TourName,
                Place = booking.Place,
                Email = booking.Email,
                FirstName = booking.FirstName,
                TourId = booking.TourId,
                CreatedDate = booking.CreatedDate,
                IsActive = booking.IsActive
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking details for ID {BookingId}", id);
            return RedirectToPage("./MyBookings");
        }
    }
}
