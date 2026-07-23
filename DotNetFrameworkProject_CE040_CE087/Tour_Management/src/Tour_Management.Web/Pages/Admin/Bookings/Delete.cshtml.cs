using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin.Bookings;

/// <summary>
/// Page model for deleting a booking (admin).
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets or sets the booking to delete.</summary>
    public BookingDeleteViewModel? Booking { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteModel"/> class.
    /// </summary>
    public DeleteModel(IBookingService bookingService, ILogger<DeleteModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the booking delete confirmation page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
        if (dto == null)
            return NotFound();

        Booking = new BookingDeleteViewModel
        {
            BookingId = dto.BookingId,
            TourName = dto.TourName,
            Place = dto.Place,
            Email = dto.Email,
            FirstName = dto.FirstName
        };
        return Page();
    }

    /// <summary>Handles POST requests for the booking deletion.</summary>
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        try
        {
            await _bookingService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Admin deleted booking id {BookingId}", id);
            TempData["Success"] = "Booking deleted successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking id {BookingId}", id);
            TempData["Error"] = "An error occurred while deleting the booking.";
            return RedirectToPage("./Index");
        }
    }
}
