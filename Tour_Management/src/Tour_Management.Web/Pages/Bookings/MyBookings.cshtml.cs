using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for user's own bookings.
/// </summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        try
        {
            var bookings = await _bookingService.GetBookingsByEmailAsync(email);
            Bookings = bookings.Select(b => new BookingViewModel
            {
                BookingId = b.BookingId,
                TourName = b.TourName,
                Place = b.Place,
                Email = b.Email,
                FirstName = b.FirstName,
                CreatedDate = b.CreatedDate
            });
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings for user: {Email}", email);
            return Page();
        }
    }
}
