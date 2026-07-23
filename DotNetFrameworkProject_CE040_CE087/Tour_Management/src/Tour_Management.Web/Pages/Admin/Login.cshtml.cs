using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin;

/// <summary>
/// Page model for admin login.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [BindProperty]
    public AdminLoginViewModel Input { get; set; } = new();

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("IsAdmin") == "true")
            return RedirectToPage("./Dashboard");
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
            HttpContext.Session.SetString("AdminEmail", Input.Email);
            _logger.LogInformation("Admin logged in: {Email}", Input.Email);
            return RedirectToPage("./Dashboard");
        }

        ModelState.AddModelError(string.Empty, "Invalid admin credentials.");
        return Page();
    }
}
