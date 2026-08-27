using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for cancelling/deleting a booking.
/// </summary>
[Authorize]
public class DeleteModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets or sets the booking delete view model.</summary>
    [BindProperty]
    public BookingDeleteViewModel? Booking { get; set; }

    /// <summary>Initializes a new instance of DeleteModel.</summary>
    public DeleteModel(IBookingService bookingService, ILogger<DeleteModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the delete booking confirmation page.</summary>
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
                FirstName = dto.FirstName,
                Email = dto.Email,
                BookingDate = dto.BookingDate
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for delete, ID {BookingId}", id);
            return RedirectToPage("Index");
        }
    }

    /// <summary>Handles POST requests to delete a booking.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (Booking == null)
            return RedirectToPage("Index");

        try
        {
            await _bookingService.DeleteAsync(Booking.BookingId, cancellationToken);
            TempData["SuccessMessage"] = "Booking cancelled successfully!";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with ID {BookingId}", Booking.BookingId);
            TempData["ErrorMessage"] = "An error occurred while cancelling the booking.";
            return RedirectToPage("Index");
        }
    }
}
