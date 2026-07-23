using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for viewing user's own bookings.
/// </summary>
public class MyBookingsModel : PageModel
{
    private readonly BookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    public MyBookingsModel(BookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public IEnumerable<BookingListViewModel> Bookings { get; set; } = Enumerable.Empty<BookingListViewModel>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var userEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(userEmail))
            return RedirectToPage("/Users/Login");

        try
        {
            var dtos = await _bookingService.GetByEmailAsync(userEmail, cancellationToken);

            // Manual mapping from DTO to ViewModel
            Bookings = dtos.Select(dto => new BookingListViewModel
            {
                BookingId = dto.BookingId,
                TourName = dto.TourName,
                Place = dto.Place,
                Email = dto.Email,
                FirstName = dto.FirstName,
                BookingDate = dto.BookingDate
            });
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings for user: {Email}", userEmail);
            Bookings = Enumerable.Empty<BookingListViewModel>();
            return Page();
        }
    }
}
