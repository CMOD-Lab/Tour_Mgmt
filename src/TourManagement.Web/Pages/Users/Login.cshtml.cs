using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for user login.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    [BindProperty]
    public LoginViewModel Input { get; set; } = new();

    public string ErrorMessage { get; set; } = string.Empty;

    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        // Redirect if already logged in
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            return RedirectToPage("/Tours/Index");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var user = await _userService.AuthenticateAsync(Input.Email, Input.Password, cancellationToken);

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserFirstName", user.FirstName);
                _logger.LogInformation("User {Email} logged in successfully", Input.Email);
                return RedirectToPage("/Tours/Index");
            }
            else
            {
                ErrorMessage = "Invalid email or password. Please try again.";
                _logger.LogWarning("Failed login attempt for email {Email}", Input.Email);
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email {Email}", Input.Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
