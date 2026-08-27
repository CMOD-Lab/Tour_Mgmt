using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for the Bookings index/list page.
/// </summary>
[Authorize]
public class IndexModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets the list of bookings to display.</summary>
    public IEnumerable<BookingListViewModel> Bookings { get; private set; } = Enumerable.Empty<BookingListViewModel>();

    /// <summary>Initializes a new instance of IndexModel.</summary>
    public IndexModel(IBookingService bookingService, ILogger<IndexModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the bookings list page.</summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var userEmail = User.Identity?.Name;
            var isAdmin = User.IsInRole("Admin");

            var bookings = isAdmin
                ? await _bookingService.GetAllAsync(cancellationToken)
                : await _bookingService.GetByEmailAsync(userEmail ?? string.Empty, cancellationToken);

            // Manual mapping from DTO to ViewModel
            Bookings = bookings.Select(b => new BookingListViewModel
            {
                BookingId = b.BookingId,
                TourName = b.TourName,
                Place = b.Place,
                Email = b.Email,
                FirstName = b.FirstName,
                BookingDate = b.BookingDate
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings list");
            TempData["ErrorMessage"] = "An error occurred while loading bookings.";
        }
    }
}
