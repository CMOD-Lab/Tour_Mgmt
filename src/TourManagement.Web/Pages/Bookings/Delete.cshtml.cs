using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for booking delete/cancel confirmation.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    public BookingDeleteViewModel? Booking { get; set; }

    public DeleteModel(IBookingService bookingService, ILogger<DeleteModel> logger)
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
            Booking = new BookingDeleteViewModel
            {
                BookingId = booking.BookingId,
                TourName = booking.TourName,
                Email = booking.Email,
                FirstName = booking.FirstName
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for delete, ID {BookingId}", id);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("UserEmail") == null)
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            await _bookingService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Booking {BookingId} cancelled", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking {BookingId}", id);
        }

        return RedirectToPage("/Bookings/MyBookings");
    }
}
