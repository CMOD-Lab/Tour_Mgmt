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
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the admin password.</summary>
    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
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
    public void OnGet()
    {
    }

    /// <summary>
    /// Handles POST requests to authenticate the admin.
    /// </summary>
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var adminEmail = _configuration["AppSettings:AdminEmail"] ?? "admin@gmail.com";
        var adminPassword = _configuration["AppSettings:AdminPassword"] ?? "admin";

        if (Email == adminEmail && Password == adminPassword)
        {
            HttpContext.Session.SetString("UserEmail", Email);
            HttpContext.Session.SetString("UserName", "Administrator");
            HttpContext.Session.SetString("UserRole", "Admin");
            _logger.LogInformation("Admin logged in: {Email}", Email);
            TempData["SuccessMessage"] = "Welcome, Administrator!";
            return RedirectToPage("Index");
        }

        ModelState.AddModelError(string.Empty, "Invalid admin credentials.");
        return Page();
    }
}
