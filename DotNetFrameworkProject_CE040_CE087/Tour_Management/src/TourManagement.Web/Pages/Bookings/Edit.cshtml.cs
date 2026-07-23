using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for editing an existing booking.
/// </summary>
public class EditModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the input view model.</summary>
    [BindProperty]
    public BookingEditViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EditModel"/> class.
    /// </summary>
    public EditModel(IBookingService bookingService, ILogger<EditModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the edit booking page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var booking = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (booking == null)
                return NotFound();

            // Map Entity to ViewModel manually
            Input = new BookingEditViewModel
            {
                Id = booking.Id,
                TourName = booking.TourName,
                Place = booking.Place,
                Email = booking.Email,
                FirstName = booking.FirstName,
                IsActive = booking.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading edit page for booking id {BookingId}", id);
            return RedirectToPage("Index");
        }
    }

    /// <summary>Handles POST requests to update a booking.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var existingBooking = await _bookingService.GetByIdAsync(Input.Id, cancellationToken);
            if (existingBooking == null)
                return NotFound();

            // Update entity properties
            existingBooking.TourName = Input.TourName;
            existingBooking.Place = Input.Place;
            existingBooking.Email = Input.Email;
            existingBooking.FirstName = Input.FirstName;
            existingBooking.IsActive = Input.IsActive;
            existingBooking.ModifiedBy = User.Identity?.Name ?? "system";

            await _bookingService.UpdateAsync(existingBooking, cancellationToken);
            TempData["SuccessMessage"] = "Booking updated successfully!";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking with id {BookingId}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the booking. Please try again.");
            return Page();
        }
    }
}
