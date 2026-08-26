using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TourBooking.Web.Pages.Admin;

/// <summary>
/// Page model for the admin login page.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>Gets or sets the admin email.</summary>
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the admin password.</summary>
    [BindProperty]
    public string Password { get; set; } = string.Empty;

    /// <summary>Gets or sets the error message.</summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Initializes a new instance of the <see cref="LoginModel"/> class.</summary>
    public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the admin login page.</summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") == "true")
        {
            return RedirectToPage("/Admin/Dashboard");
        }
        return Page();
    }

    /// <summary>Handles POST requests for the admin login form submission.</summary>
    public IActionResult OnPost()
    {
        var adminEmail = _configuration["AdminCredentials:Email"] ?? "admin@gmail.com";
        var adminPassword = _configuration["AdminCredentials:Password"] ?? "admin";

        if (Email == adminEmail && Password == adminPassword)
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("UserEmail", Email);
            _logger.LogInformation("Admin logged in successfully");
            return RedirectToPage("/Admin/Dashboard");
        }

        ErrorMessage = "Invalid admin credentials.";
        _logger.LogWarning("Failed admin login attempt for email: {Email}", Email);
        return Page();
    }
}
