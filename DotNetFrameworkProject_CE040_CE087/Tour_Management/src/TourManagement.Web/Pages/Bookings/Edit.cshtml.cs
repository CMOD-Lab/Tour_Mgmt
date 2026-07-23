using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for editing an existing booking.
/// </summary>
public class EditModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the booking edit view model.</summary>
    [BindProperty]
    public BookingEditViewModel Booking { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="EditModel"/>.
    /// </summary>
    public EditModel(IBookingService bookingService, ILogger<EditModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the edit booking page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
                return NotFound();

            Booking = new BookingEditViewModel
            {
                Id = dto.Id,
                TourName = dto.TourName,
                Place = dto.Place,
                Email = dto.Email,
                FirstName = dto.FirstName
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for edit, id {BookingId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the booking.";
            return RedirectToPage("Index");
        }
    }

    /// <summary>
    /// Handles POST requests to update a booking.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var dto = new BookingUpdateDto
            {
                TourName = Booking.TourName,
                Place = Booking.Place,
                Email = Booking.Email,
                FirstName = Booking.FirstName,
                ModifiedBy = HttpContext.Session.GetString("UserEmail") ?? "system"
            };

            await _bookingService.UpdateAsync(Booking.Id, dto, cancellationToken);
            TempData["SuccessMessage"] = "Booking updated successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking with id {BookingId}", Booking.Id);
            TempData["ErrorMessage"] = "An error occurred while updating the booking.";
            return Page();
        }
    }
}
