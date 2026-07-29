using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

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

    [BindProperty]
    public BookingViewModel? Booking { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        var booking = await _bookingService.GetBookingByIdAsync(id, cancellationToken);
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

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        if (Booking == null)
            return NotFound();

        try
        {
            await _bookingService.DeleteBookingAsync(Booking.BookingId, cancellationToken);
            TempData["SuccessMessage"] = "Booking cancelled successfully!";
            return RedirectToPage("./MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with ID {BookingId}", Booking.BookingId);
            ModelState.AddModelError(string.Empty, "An error occurred while cancelling the booking.");
            return Page();
        }
    }
}
