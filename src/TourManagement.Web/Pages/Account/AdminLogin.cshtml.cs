using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Account;

/// <summary>
/// Page model for the admin login page.
/// </summary>
public class AdminLoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AdminLoginModel> _logger;

    [BindProperty]
    public LoginViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public AdminLoginModel(IConfiguration configuration, ILogger<AdminLoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var adminEmail = _configuration["AppSettings:AdminEmail"] ?? "admin@gmail.com";
        var adminPassword = _configuration["AppSettings:AdminPassword"] ?? "admin";

        if (Input.Email == adminEmail && Input.Password == adminPassword)
        {
            HttpContext.Session.SetString("UserEmail", Input.Email);
            HttpContext.Session.SetString("UserFirstName", "Admin");
            HttpContext.Session.SetString("IsAdmin", "true");

            _logger.LogInformation("Admin logged in successfully.");
            return RedirectToPage("/Admin/Dashboard");
        }

        ErrorMessage = "Invalid admin credentials.";
        _logger.LogWarning("Failed admin login attempt for email {Email}.", Input.Email);
        return Page();
    }
}
