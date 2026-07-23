using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin;

/// <summary>
/// Page model for admin login.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModel"/> class.
    /// </summary>
    public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>Gets or sets the admin login view model.</summary>
    [BindProperty]
    public AdminLoginViewModel Login { get; set; } = new();

    /// <summary>
    /// Handles GET requests for the admin login page.
    /// </summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") == "true")
        {
            return RedirectToPage("./Index");
        }
        return Page();
    }

    /// <summary>
    /// Handles POST requests to authenticate an admin user.
    /// </summary>
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Validate admin credentials from configuration
            var adminEmail = _configuration["AppSettings:AdminEmail"] ?? "admin@gmail.com";
            var adminPassword = _configuration["AppSettings:AdminPassword"] ?? "admin";

            if (Login.Email == adminEmail && Login.Password == adminPassword)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                HttpContext.Session.SetString("UserEmail", Login.Email);
                _logger.LogInformation("Admin logged in: {Email}", Login.Email);
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid admin credentials.");
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during admin login");
            ModelState.AddModelError(string.Empty, "An error occurred during login.");
            return Page();
        }
    }
}
