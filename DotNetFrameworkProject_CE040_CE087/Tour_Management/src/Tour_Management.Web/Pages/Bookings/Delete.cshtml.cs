using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for deleting a booking.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteModel"/> class.
    /// </summary>
    public DeleteModel(IBookingService bookingService, ILogger<DeleteModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Gets or sets the booking delete view model.</summary>
    [BindProperty]
    public BookingDeleteViewModel? Booking { get; set; }

    /// <summary>
    /// Handles GET requests for the delete booking confirmation page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var booking = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (booking == null)
            {
                return NotFound();
            }

            // Manual ViewModel mapping
            Booking = new BookingDeleteViewModel
            {
                BookingId = booking.BookingId,
                TourName = booking.TourName,
                Email = booking.Email,
                FirstName = booking.FirstName
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for delete, id {BookingId}", id);
            return RedirectToPage("./Index");
        }
    }

    /// <summary>
    /// Handles POST requests to delete a booking.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        if (Booking == null)
        {
            return RedirectToPage("./Index");
        }

        try
        {
            await _bookingService.DeleteAsync(Booking.BookingId, cancellationToken);
            _logger.LogInformation("Booking deleted: {BookingId}", Booking.BookingId);

            TempData["Message"] = "Booking was deleted successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking {BookingId}", Booking.BookingId);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the booking.");
            return Page();
        }
    }
}
