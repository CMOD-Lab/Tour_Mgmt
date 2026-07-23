using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Bookings;

/// <summary>
/// Page model for viewing the current user's bookings.
/// </summary>
public class MyBookingsModel : PageModel
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<MyBookingsModel> _logger;

    /// <summary>Gets or sets the list of bookings for the current user.</summary>
    public IEnumerable<BookingViewModel> Bookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    /// <summary>
    /// Initializes a new instance of <see cref="MyBookingsModel"/>.
    /// </summary>
    public MyBookingsModel(IBookingService bookingService, ILogger<MyBookingsModel> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the my bookings page.
    /// </summary>
    public async Task OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (!string.IsNullOrEmpty(email))
            {
                var dtos = await _bookingService.GetByEmailAsync(email, cancellationToken);
                Bookings = dtos.Select(dto => new BookingViewModel
                {
                    Id = dto.Id,
                    TourName = dto.TourName,
                    Place = dto.Place,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    BookingDate = dto.BookingDate,
                    TourId = dto.TourId,
                    CreatedDate = dto.CreatedDate,
                    IsActive = dto.IsActive
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user bookings");
            TempData["ErrorMessage"] = "An error occurred while loading your bookings.";
        }
    }
}
