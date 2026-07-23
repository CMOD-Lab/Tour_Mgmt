using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Booking;

/// <summary>
/// My bookings page model - migrated from mybooking.aspx.
/// </summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/User/Login");
        }

        try
        {
            var bookingDtos = await _bookingService.GetByEmailAsync(email, cancellationToken);

            // Manual mapping from DTO to ViewModel
            Bookings = bookingDtos.Select(b => new BookingViewModel
            {
                BookingId = b.BookingId,
                TourName = b.TourName,
                Place = b.Place,
                Email = b.Email,
                FirstName = b.FirstName,
                BookingDate = b.BookingDate
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings for user: {Email}", email);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCancelAsync(int bookingId, CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/User/Login");
        }

        try
        {
            await _bookingService.DeleteAsync(bookingId, cancellationToken);
            _logger.LogInformation("Booking cancelled: ID {BookingId} by user {Email}", bookingId, email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking ID: {BookingId}", bookingId);
        }

        return RedirectToPage();
    }
}
