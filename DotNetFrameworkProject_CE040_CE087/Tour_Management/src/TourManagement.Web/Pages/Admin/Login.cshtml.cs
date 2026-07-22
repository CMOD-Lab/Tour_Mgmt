using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.Pages.Admin;

/// <summary>
/// Page model for admin login.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>Gets or sets the admin email.</summary>
    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email is required.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the admin password.</summary>
    [BindProperty]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of <see cref="LoginModel"/>.
    /// </summary>
    public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the admin login page.
    /// </summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") == "true")
            return RedirectToPage("Dashboard");
        return Page();
    }

    /// <summary>
    /// Handles POST requests for admin login.
    /// </summary>
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var adminEmail = _configuration["AdminCredentials:Email"] ?? "admin@gmail.com";
        var adminPassword = _configuration["AdminCredentials:Password"] ?? "admin";

        if (Email == adminEmail && Password == adminPassword)
        {
            HttpContext.Session.SetString("UserEmail", Email);
            HttpContext.Session.SetString("UserName", "Administrator");
            HttpContext.Session.SetString("IsAdmin", "true");
            _logger.LogInformation("Admin logged in successfully");
            return RedirectToPage("Dashboard");
        }

        ModelState.AddModelError(string.Empty, "Invalid admin credentials.");
        return Page();
    }
}
