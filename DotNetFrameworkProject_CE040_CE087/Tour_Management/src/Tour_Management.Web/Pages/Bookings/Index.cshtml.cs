using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Bookings;

/// <summary>
/// Page model for the bookings index/list page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    public IndexModel(IBookingService bookingService, ILogger<IndexModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>Gets the list of bookings to display.</summary>
    public IEnumerable<BookingIndexViewModel> Bookings { get; private set; } = Enumerable.Empty<BookingIndexViewModel>();

    /// <summary>Gets or sets the success message.</summary>
    public string? Message { get; set; }

    /// <summary>
    /// Handles GET requests for the bookings list page.
    /// </summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Loading bookings list");
            Message = TempData["Message"]?.ToString();

            IEnumerable<Domain.Entities.Booking> bookings;
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

            if (isAdmin)
            {
                bookings = await _bookingService.GetAllAsync(cancellationToken);
            }
            else if (!string.IsNullOrEmpty(userEmail))
            {
                bookings = await _bookingService.GetByEmailAsync(userEmail, cancellationToken);
            }
            else
            {
                bookings = Enumerable.Empty<Domain.Entities.Booking>();
            }

            // Manual ViewModel mapping
            Bookings = bookings.Select(b => new BookingIndexViewModel
            {
                BookingId = b.BookingId,
                TourName = b.TourName,
                Place = b.Place,
                Email = b.Email,
                FirstName = b.FirstName,
                CreatedDate = b.CreatedDate,
                IsActive = b.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading bookings list");
            ModelState.AddModelError(string.Empty, "An error occurred while loading bookings.");
        }
    }
}
