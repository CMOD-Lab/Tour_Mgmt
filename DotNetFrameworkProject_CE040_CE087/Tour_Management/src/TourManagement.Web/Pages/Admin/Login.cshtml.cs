using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Admin;

/// <summary>
/// Page model for admin login.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>Gets or sets the input view model.</summary>
    [BindProperty]
    public UserLoginViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModel"/> class.
    /// </summary>
    public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the admin login page.</summary>
    public IActionResult OnGet()
    {
        return Page();
    }

    /// <summary>Handles POST requests to authenticate an admin user.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Validate admin credentials from configuration
            var adminEmail = _configuration["AdminSettings:Email"] ?? "admin@gmail.com";
            var adminPassword = _configuration["AdminSettings:Password"] ?? "admin";

            if (Input.Email != adminEmail || Input.Password != adminPassword)
            {
                ModelState.AddModelError(string.Empty, "Invalid admin credentials.");
                return Page();
            }

            // Create admin claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Input.Email),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("IsAdmin", "true")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(4)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            _logger.LogInformation("Admin {Email} logged in successfully", Input.Email);
            return RedirectToPage("/Admin/Dashboard");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during admin login for: {Email}", Input.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during login. Please try again.");
            return Page();
        }
    }
}
