using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Admin;

/// <summary>
/// Page model for the admin delete user page.
/// </summary>
public class DeleteUserModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<DeleteUserModel> _logger;

    /// <summary>Gets or sets the user profile view model.</summary>
    public UserProfileViewModel? UserProfile { get; set; }

    /// <summary>Initializes a new instance of the <see cref="DeleteUserModel"/> class.</summary>
    public DeleteUserModel(IUserService userService, ILogger<DeleteUserModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the delete user page.</summary>
    public async Task<IActionResult> OnGetAsync(string email, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            var user = await _userService.GetByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }

            UserProfile = new UserProfileViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Dob = user.Dob,
                Street = user.Street,
                City = user.City,
                State = user.State
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user for deletion: {Email}", email);
            return RedirectToPage("/Admin/Users");
        }
    }

    /// <summary>Handles POST requests for the delete user form submission.</summary>
    public async Task<IActionResult> OnPostAsync(string email, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            await _userService.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("User deleted: {Email}", email);
            return RedirectToPage("/Admin/Users");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {Email}", email);
            return RedirectToPage("/Admin/Users");
        }
    }
}
