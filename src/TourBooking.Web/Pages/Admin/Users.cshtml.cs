using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Admin;

/// <summary>
/// Page model for the admin users management page.
/// </summary>
public class UsersModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersModel> _logger;

    /// <summary>Gets or sets the list of users.</summary>
    public IEnumerable<UserProfileViewModel> Users { get; set; } = Enumerable.Empty<UserProfileViewModel>();

    /// <summary>Initializes a new instance of the <see cref="UsersModel"/> class.</summary>
    public UsersModel(IUserService userService, ILogger<UsersModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the admin users page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var users = await _userService.GetAllAsync(cancellationToken);

            Users = users.Select(u => new UserProfileViewModel
            {
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Gender = u.Gender,
                Dob = u.Dob,
                Street = u.Street,
                City = u.City,
                State = u.State
            });

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin users");
            return Page();
        }
    }
}
