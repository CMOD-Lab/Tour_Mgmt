using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for displaying the current user's bookings.
/// </summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    /// <summary>Gets or sets the list of bookings for the current user.</summary>
    public IEnumerable<BookingIndexViewModel> Bookings { get; set; } = Enumerable.Empty<BookingIndexViewModel>();

    /// <summary>
    /// Initializes a new instance of the <see cref="MyBookingsModel"/> class.
    /// </summary>
    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the my bookings page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("/Users/Login");

        try
        {
            var dtos = await _bookingService.GetByEmailAsync(email, cancellationToken);
            Bookings = dtos.Select(b => new BookingIndexViewModel
            {
                BookingId = b.BookingId,
                TourName = b.TourName,
                Place = b.Place,
                Email = b.Email,
                FirstName = b.FirstName,
                CreatedDate = b.CreatedDate,
                IsActive = b.IsActive
            });
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings for email {Email}", email);
            TempData["Error"] = "An error occurred while loading your bookings.";
            return Page();
        }
    }
}
