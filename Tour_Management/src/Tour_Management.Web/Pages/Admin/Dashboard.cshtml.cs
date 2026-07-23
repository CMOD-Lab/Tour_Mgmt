using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin;

/// <summary>
/// Page model for the admin dashboard.
/// </summary>
public class DashboardModel : PageModel
{
    private readonly ITourService _tourService;
    private readonly IBookingService _bookingService;
    private readonly IUserService _userService;
    private readonly ILogger<DashboardModel> _logger;

    public DashboardModel(
        ITourService tourService,
        IBookingService bookingService,
        IUserService userService,
        ILogger<DashboardModel> logger)
    {
        _tourService = tourService;
        _bookingService = bookingService;
        _userService = userService;
        _logger = logger;
    }

    public int TotalTours { get; set; }
    public int TotalBookings { get; set; }
    public int TotalUsers { get; set; }
    public IEnumerable<BookingViewModel> RecentBookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        try
        {
            var tours = await _tourService.GetAllToursAsync();
            var bookings = await _bookingService.GetAllBookingsAsync();
            var users = await _userService.GetAllUsersAsync();

            TotalTours = tours.Count();
            TotalBookings = bookings.Count();
            TotalUsers = users.Count();

            RecentBookings = bookings.Take(5).Select(b => new BookingViewModel
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
            _logger.LogError(ex, "Error loading admin dashboard");
            return Page();
        }
    }
}
