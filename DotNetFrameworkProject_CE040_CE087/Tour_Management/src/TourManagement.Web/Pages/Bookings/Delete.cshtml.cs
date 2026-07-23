using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for cancelling/deleting a booking.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets or sets the booking view model.</summary>
    [BindProperty]
    public BookingViewModel? Booking { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteModel"/>.
    /// </summary>
    public DeleteModel(IBookingService bookingService, ILogger<DeleteModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the delete booking confirmation page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
                return NotFound();

            Booking = new BookingViewModel
            {
                Id = dto.Id,
                TourName = dto.TourName,
                Place = dto.Place,
                Email = dto.Email,
                FirstName = dto.FirstName,
                BookingDate = dto.BookingDate
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for delete, id {BookingId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the booking.";
            return RedirectToPage("Index");
        }
    }

    /// <summary>
    /// Handles POST requests to delete a booking.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (Booking is null)
            return RedirectToPage("Index");

        try
        {
            await _bookingService.DeleteAsync(Booking.Id, cancellationToken);
            TempData["SuccessMessage"] = "Booking cancelled successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with id {BookingId}", Booking.Id);
            TempData["ErrorMessage"] = "An error occurred while cancelling the booking.";
            return RedirectToPage("Index");
        }
    }
}
