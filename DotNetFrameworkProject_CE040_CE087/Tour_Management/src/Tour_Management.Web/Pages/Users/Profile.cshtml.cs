using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for the user profile page.
/// </summary>
public class ProfileModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<ProfileModel> _logger;

    /// <summary>Gets or sets the user profile to display.</summary>
    public UserProfileViewModel? UserProfile { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProfileModel"/> class.
    /// </summary>
    public ProfileModel(IUserService userService, ILogger<ProfileModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the profile page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var email = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(email))
            return RedirectToPage("./Login");

        try
        {
            var user = await _userService.GetByEmailAsync(email, cancellationToken);
            if (user == null)
                return RedirectToPage("./Login");

            UserProfile = new UserProfileViewModel
            {
                UserId = user.UserId,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Street = user.Street,
                City = user.City,
                State = user.State
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for email {Email}", email);
            TempData["Error"] = "An error occurred while loading your profile.";
            return RedirectToPage("/Index");
        }
    }
}
