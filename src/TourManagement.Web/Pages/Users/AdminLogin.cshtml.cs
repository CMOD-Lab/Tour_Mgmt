using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for admin login.
/// </summary>
public class AdminLoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AdminLoginModel> _logger;

    public AdminLoginModel(IConfiguration configuration, ILogger<AdminLoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [BindProperty]
    public UserLoginViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") == "true")
            return RedirectToPage("/Admin/Dashboard");
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        var adminEmail = _configuration["AdminCredentials:Email"] ?? "admin@gmail.com";
        var adminPassword = _configuration["AdminCredentials:Password"] ?? "admin";

        if (Input.Email == adminEmail && Input.Password == adminPassword)
        {
            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetString("UserEmail", Input.Email);
            HttpContext.Session.SetString("UserName", "Admin");
            _logger.LogInformation("Admin logged in successfully");
            return RedirectToPage("/Admin/Dashboard");
        }

        ErrorMessage = "Invalid admin credentials.";
        return Page();
    }
}
