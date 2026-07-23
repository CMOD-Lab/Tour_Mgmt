using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Account;

/// <summary>
/// Page model for the user login page.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    [BindProperty]
    public LoginViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public void OnGet(string? message = null)
    {
        if (!string.IsNullOrEmpty(message))
        {
            SuccessMessage = message;
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var user = await _userService.AuthenticateAsync(Input.Email, Input.Password, cancellationToken);

            if (user is null)
            {
                ErrorMessage = "Invalid email or password. Please try again.";
                _logger.LogWarning("Failed login attempt for email {Email}.", Input.Email);
                return Page();
            }

            // Store user info in session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetString("IsAdmin", "false");

            _logger.LogInformation("User {Email} logged in successfully.", Input.Email);
            return RedirectToPage("/Account/Profile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email {Email}.", Input.Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
