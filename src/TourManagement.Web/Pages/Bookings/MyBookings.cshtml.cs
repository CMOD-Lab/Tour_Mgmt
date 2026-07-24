using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>Page model for the user's bookings page.</summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    /// <summary>Gets or sets the list of user's bookings.</summary>
    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    /// <summary>Initializes a new instance of <see cref="MyBookingsModel"/>.</summary>
    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the user's bookings page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            var bookings = await _bookingService.GetByEmailAsync(email, cancellationToken);

            // Manual mapping from DTO to ViewModel
            Bookings = bookings.Select(b => new BookingViewModel
            {
                BookingId = b.BookingId,
                TourName = b.TourName,
                Place = b.Place,
                Email = b.Email,
                FirstName = b.FirstName,
                BookingDate = b.BookingDate
            });

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings for email {Email}", email);
            return Page();
        }
    }

    /// <summary>Handles POST requests for cancelling a booking.</summary>
    public async Task<IActionResult> OnPostCancelAsync(int id, CancellationToken cancellationToken)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            await _bookingService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("User {Email} cancelled booking ID {BookingId}", email, id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling booking ID {BookingId}", id);
        }

        return RedirectToPage();
    }
}
