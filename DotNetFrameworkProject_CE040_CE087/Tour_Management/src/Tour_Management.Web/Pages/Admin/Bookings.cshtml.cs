using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin;

/// <summary>
/// Page model for admin bookings management.
/// </summary>
public class BookingsModel : PageModel
{
    private readonly BookingService _bookingService;
    private readonly ILogger<BookingsModel> _logger;

    public BookingsModel(BookingService bookingService, ILogger<BookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public IEnumerable<BookingListViewModel> Bookings { get; set; } = Enumerable.Empty<BookingListViewModel>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("./Login");

        try
        {
            var dtos = await _bookingService.GetAllAsync(cancellationToken);

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
            _logger.LogError(ex, "Error loading all bookings for admin");
            Bookings = Enumerable.Empty<BookingListViewModel>();
            return Page();
        }
    }
}
