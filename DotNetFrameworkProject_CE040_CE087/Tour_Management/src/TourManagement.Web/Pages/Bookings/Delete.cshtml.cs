using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for cancelling/deleting a booking.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets the booking to be cancelled.</summary>
    public BookingDto? Booking { get; private set; }

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteModel"/>.
    /// </summary>
    public DeleteModel(IBookingService bookingService, ILogger<DeleteModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the cancel booking confirmation page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            Booking = await _bookingService.GetByIdAsync(id);
            if (Booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToPage("MyBookings");
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for cancel, id {BookingId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the booking.";
            return RedirectToPage("MyBookings");
        }
    }

    /// <summary>
    /// Handles POST requests to cancel a booking.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(int id)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            await _bookingService.DeleteAsync(id);
            TempData["SuccessMessage"] = "Booking was cancelled successfully.";
            return RedirectToPage("MyBookings");
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "Booking not found.";
            return RedirectToPage("MyBookings");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking with id {BookingId}", id);
            TempData["ErrorMessage"] = "An error occurred while cancelling the booking.";
            return RedirectToPage("MyBookings");
        }
    }
}
