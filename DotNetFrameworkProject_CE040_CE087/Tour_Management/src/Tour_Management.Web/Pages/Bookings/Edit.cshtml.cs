using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>Page model for editing a booking.</summary>
public class EditModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IBookingService bookingService, ILogger<EditModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    [BindProperty]
    public BookingEditViewModel Booking { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            var dto = await _bookingService.GetByIdAsync(id, cancellationToken);
            if (dto == null) return NotFound();

            Booking = BookingEditViewModel.FromDto(dto);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading booking for edit, ID {BookingId}", id);
            return RedirectToPage("./MyBookings");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
        {
            return RedirectToPage("/Users/Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var updateDto = Booking.ToUpdateDto();
            var result = await _bookingService.UpdateAsync(Booking.BookingId, updateDto, cancellationToken);

            if (result == null) return NotFound();

            TempData["SuccessMessage"] = "Booking updated successfully!";
            return RedirectToPage("./Details", new { id = Booking.BookingId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking with ID {BookingId}", Booking.BookingId);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the booking. Please try again.");
            return Page();
        }
    }
}
