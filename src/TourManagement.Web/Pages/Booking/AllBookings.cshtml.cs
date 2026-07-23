using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Booking;

/// <summary>
/// All bookings page model - migrated from allbooking.aspx.
/// </summary>
public class AllBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<AllBookingsModel> _logger;

    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();
    public string? SearchTerm { get; set; }

    public AllBookingsModel(IBookingService bookingService, ILogger<AllBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string? searchTerm, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        SearchTerm = searchTerm;

        try
        {
            var bookingDtos = string.IsNullOrWhiteSpace(searchTerm)
                ? await _bookingService.GetAllAsync(cancellationToken)
                : await _bookingService.SearchAsync(searchTerm, cancellationToken);

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
            _logger.LogError(ex, "Error loading all bookings");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int bookingId, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            await _bookingService.DeleteAsync(bookingId, cancellationToken);
            _logger.LogInformation("Booking deleted by admin: ID {BookingId}", bookingId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking ID: {BookingId}", bookingId);
        }

        return RedirectToPage();
    }
}
