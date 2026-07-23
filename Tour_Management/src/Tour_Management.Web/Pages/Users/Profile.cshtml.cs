using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for user profile page.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;
    private readonly ILogger<ProfileModel> _logger;

    public ProfileModel(IUserService userService, IBookingService bookingService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _bookingService = bookingService;
        _logger = logger;
    }

    public UserViewModel? User { get; set; }
    public IEnumerable<BookingViewModel> RecentBookings { get; set; } = Enumerable.Empty<BookingViewModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("Login");

        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user != null)
            {
                User = new UserViewModel
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    DateOfBirth = user.DateOfBirth,
                    Street = user.Street,
                    City = user.City,
                    State = user.State
                };

                var bookings = await _bookingService.GetBookingsByEmailAsync(email);
                RecentBookings = bookings.Take(5).Select(b => new BookingViewModel
                {
                    BookingId = b.BookingId,
                    TourName = b.TourName,
                    Place = b.Place,
                    Email = b.Email,
                    FirstName = b.FirstName,
                    CreatedDate = b.CreatedDate
                });
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for user: {Email}", email);
            return Page();
        }
    }
}
