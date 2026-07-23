using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for editing an existing booking.
/// </summary>
public class EditModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<EditModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EditModel"/> class.
    /// </summary>
    public EditModel(IBookingService bookingService, ILogger<EditModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Gets or sets the booking edit view model.</summary>
    [BindProperty]
    public BookingEditViewModel Booking { get; set; } = new();

    /// <summary>
    /// Handles GET requests for the edit booking page.
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
            Booking = new BookingEditViewModel
            {
                BookingId = booking.BookingId,
                TourName = booking.TourName,
                Place = booking.Place,
                Email = booking.Email,
                FirstName = booking.FirstName,
                IsActive = booking.IsActive,
                TourId = booking.TourId
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for edit, id {BookingId}", id);
            return RedirectToPage("./Index");
        }
    }

    /// <summary>
    /// Handles POST requests to update a booking.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Manual mapping from ViewModel to Domain entity
            var booking = new Booking
            {
                BookingId = Booking.BookingId,
                TourName = Booking.TourName,
                Place = Booking.Place,
                Email = Booking.Email,
                FirstName = Booking.FirstName,
                IsActive = Booking.IsActive,
                TourId = Booking.TourId,
                ModifiedDate = DateTime.UtcNow,
                ModifiedBy = HttpContext.Session.GetString("UserEmail") ?? "admin"
            };

            await _bookingService.UpdateAsync(booking, cancellationToken);
            _logger.LogInformation("Booking updated: {BookingId}", booking.BookingId);

            TempData["Message"] = "Booking was updated successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking {BookingId}", Booking.BookingId);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the booking.");
            return Page();
        }
    }
}
