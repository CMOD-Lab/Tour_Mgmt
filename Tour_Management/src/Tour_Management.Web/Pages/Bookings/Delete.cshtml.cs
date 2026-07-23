using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for cancelling/deleting a booking.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IBookingService bookingService, ILogger<DeleteModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public BookingViewModel? Booking { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound();

            if (booking.Email != email && HttpContext.Session.GetString("IsAdmin") != "true")
                return Forbid();

            Booking = new BookingViewModel
            {
                BookingId = booking.BookingId,
                TourName = booking.TourName,
                Place = booking.Place,
                Email = booking.Email,
                FirstName = booking.FirstName,
                CreatedDate = booking.CreatedDate
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for delete, ID {BookingId}", id);
            return RedirectToPage("MyBookings");
        }
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        try
        {
            await _bookingService.DeleteBookingAsync(id);
            TempData["SuccessMessage"] = "Booking cancelled successfully!";
            return RedirectToPage("MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with ID {BookingId}", id);
            TempData["ErrorMessage"] = "An error occurred while cancelling the booking.";
            return RedirectToPage("MyBookings");
        }
    }
}
