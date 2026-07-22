using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for editing a booking.
/// </summary>
public class EditModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the edit input.</summary>
    [BindProperty]
    public BookingEditViewModel Input { get; set; } = new();

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
    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToPage("Index");
            }

            // Map DTO to ViewModel manually
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
            _logger.LogError(ex, "Error loading booking for edit, id {BookingId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the booking.";
            return RedirectToPage("Index");
        }
    }

    /// <summary>
    /// Handles POST requests to update a booking.
    /// </summary>
    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Map ViewModel to DTO manually
            var dto = new BookingUpdateDto
            {
                TourName = Input.TourName,
                Place = Input.Place,
                Email = Input.Email,
                FirstName = Input.FirstName,
                IsActive = Input.IsActive
            };

            await _bookingService.UpdateAsync(Input.Id, dto);
            TempData["SuccessMessage"] = "Booking updated successfully.";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "Booking not found.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking with id {BookingId}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the booking.");
            return Page();
        }
    }
}
