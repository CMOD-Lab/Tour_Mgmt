using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourBooking.Domain.Interfaces.Services;
using TourBooking.Web.ViewModels;

namespace TourBooking.Web.Pages.Users;

/// <summary>
/// Page model for the user login page.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>Gets or sets the login input model.</summary>
    [BindProperty]
    public LoginViewModel Input { get; set; } = new();

    /// <summary>Gets or sets the error message.</summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Initializes a new instance of the <see cref="LoginModel"/> class.</summary>
    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the login page.</summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            return RedirectToPage("/Users/Profile");
        }
        return Page();
    }

    /// <summary>Handles POST requests for the login form submission.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var user = await _userService.LoginAsync(Input.Email, Input.Password, cancellationToken);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password. Please try again.";
                _logger.LogWarning("Failed login attempt for email: {Email}", Input.Email);
                return Page();
            }

            // Store user info in session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserFirstName", user.FirstName);

            _logger.LogInformation("User logged in successfully: {Email}", Input.Email);
            return RedirectToPage("/Users/Profile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Input.Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
