using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for admin bookings index.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IBookingService bookingService, ILogger<IndexModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        try
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
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
            _logger.LogError(ex, "Error loading all bookings");
            return Page();
        }
    }
}
