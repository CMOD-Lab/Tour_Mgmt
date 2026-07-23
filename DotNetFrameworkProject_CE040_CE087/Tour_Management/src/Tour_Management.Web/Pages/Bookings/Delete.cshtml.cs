using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Services;
using Tour_Management.Domain.Exceptions;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for cancelling/deleting a booking.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly BookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(BookingService bookingService, ILogger<DeleteModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [BindProperty]
    public BookingDeleteViewModel? Booking { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (dto == null)
                return NotFound();

            // Manual mapping from DTO to ViewModel
            Booking = new BookingDeleteViewModel
            {
                BookingId = dto.BookingId,
                TourName = dto.TourName,
                Email = dto.Email,
                FirstName = dto.FirstName
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for delete, ID {BookingId}", id);
            return RedirectToPage("./MyBookings");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (Booking == null)
            return RedirectToPage("./MyBookings");

        try
        {
            await _bookingService.DeleteAsync(Booking.BookingId, cancellationToken);
            TempData["SuccessMessage"] = "Booking cancelled successfully.";
            return RedirectToPage("./MyBookings");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking with ID {BookingId}", Booking?.BookingId);
            TempData["ErrorMessage"] = "An error occurred while cancelling the booking.";
            return RedirectToPage("./MyBookings");
        }
    }
}
