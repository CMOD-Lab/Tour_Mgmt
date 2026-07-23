using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Tour_Management.Web.Pages.Admin;

/// <summary>
/// Page model for the admin login page.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>Gets or sets the admin email input.</summary>
    [BindProperty]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the admin password input.</summary>
    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

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
        if (HttpContext.Session.GetString("IsAdmin") == "true")
            return RedirectToPage("/Admin/Tours/Index");
        return Page();
    }

    /// <summary>Handles POST requests for the admin login form.</summary>
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var adminEmail = _configuration["AdminCredentials:Email"] ?? "admin@gmail.com";
        var adminPassword = _configuration["AdminCredentials:Password"] ?? "admin";

        if (Email == adminEmail && Password == adminPassword)
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("UserEmail", Email);
            HttpContext.Session.SetString("UserFirstName", "Admin");
            _logger.LogInformation("Admin logged in successfully");
            return RedirectToPage("/Admin/Tours/Index");
        }

        TempData["Error"] = "Invalid admin credentials.";
        _logger.LogWarning("Failed admin login attempt for email {Email}", Email);
        return Page();
    }
}
