using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for the user login page.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>Gets or sets the login input model.</summary>
    [BindProperty]
    public UserLoginViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModel"/> class.
    /// </summary>
    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the login page.</summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
            return RedirectToPage("/Users/Profile");
        return Page();
    }

    /// <summary>Handles POST requests for the login form submission.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var loginDto = new UserLoginDto
            {
                Email = Input.Email,
                Password = Input.Password
            };

            var user = await _userService.AuthenticateAsync(loginDto, cancellationToken);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }

            // Store user info in session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetInt32("UserId", user.UserId);

            _logger.LogInformation("User {Email} logged in successfully", user.Email);
            TempData["Success"] = "Login successful!";
            return RedirectToPage("/Users/Profile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email {Email}", Input.Email);
            TempData["Error"] = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
