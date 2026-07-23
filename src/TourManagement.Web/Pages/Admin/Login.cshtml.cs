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

    [BindProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email is required.")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;

    public string ErrorMessage { get; set; } = string.Empty;

    public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") == "true")
        {
            return RedirectToPage("/Admin/Dashboard");
        }
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var adminEmail = _configuration["AppSettings:AdminEmail"] ?? "admin@gmail.com";
        var adminPassword = _configuration["AppSettings:AdminPassword"] ?? "admin";

        if (Email == adminEmail && Password == adminPassword)
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("UserEmail", Email);
            HttpContext.Session.SetString("UserFirstName", "Admin");
            _logger.LogInformation("Admin logged in successfully");
            return RedirectToPage("/Admin/Dashboard");
        }

        ErrorMessage = "Invalid admin credentials.";
        _logger.LogWarning("Failed admin login attempt for email {Email}", Email);
        return Page();
    }
}
