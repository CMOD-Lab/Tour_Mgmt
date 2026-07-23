using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for user login.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>Gets or sets the login input model.</summary>
    [BindProperty]
    public UserLoginViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="LoginModel"/>.
    /// </summary>
    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the login page.
    /// </summary>
    public void OnGet()
    {
    }

    /// <summary>
    /// Handles POST requests to authenticate a user.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var user = await _userService.ValidateCredentialsAsync(Input.Email, Input.Password, cancellationToken);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            // Store user info in session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserRole", user.Role);

            TempData["SuccessMessage"] = $"Welcome back, {user.FirstName}!";
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Input.Email);
            TempData["ErrorMessage"] = "An error occurred during login.";
            return Page();
        }
    }
}
