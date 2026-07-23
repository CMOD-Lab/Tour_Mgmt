using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for user login.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IUserService userService, IConfiguration configuration, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _configuration = configuration;
        _logger = logger;
    }

    [BindProperty]
    public UserLoginViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
            return RedirectToPage("/Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Check admin credentials first
            var adminEmail = _configuration["AppSettings:AdminEmail"] ?? "admin@gmail.com";
            var adminPassword = _configuration["AppSettings:AdminPassword"] ?? "admin";

            if (Input.Email == adminEmail && Input.Password == adminPassword)
            {
                HttpContext.Session.SetString("UserEmail", Input.Email);
                HttpContext.Session.SetString("IsAdmin", "true");
                _logger.LogInformation("Admin logged in: {Email}", Input.Email);
                return RedirectToPage("/Admin/Dashboard");
            }

            // Regular user authentication
            var user = await _userService.AuthenticateAsync(Input.Email, Input.Password);
            if (user == null)
            {
                ErrorMessage = "Invalid email or password. Please try again.";
                return Page();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserFirstName", user.FirstName);
            HttpContext.Session.SetString("IsAdmin", "false");
            _logger.LogInformation("User logged in: {Email}", user.Email);
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Email}", Input.Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
