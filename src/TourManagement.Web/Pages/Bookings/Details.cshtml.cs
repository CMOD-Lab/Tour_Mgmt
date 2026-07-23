using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for booking details page.
/// </summary>
public class DetailsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DetailsModel> _logger;

    public BookingDetailsViewModel? Booking { get; set; }

    public DetailsModel(IBookingService bookingService, ILogger<DetailsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("UserEmail") == null)
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
            Booking = new BookingDetailsViewModel
            {
                BookingId = booking.BookingId,
                TourName = booking.TourName,
                Place = booking.Place,
                Email = booking.Email,
                FirstName = booking.FirstName,
                TourId = booking.TourId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking details for ID {BookingId}", id);
        }

        return Page();
    }
}
