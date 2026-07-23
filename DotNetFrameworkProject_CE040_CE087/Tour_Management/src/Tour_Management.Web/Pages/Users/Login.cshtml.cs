using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>Page model for the user login page.</summary>
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
    public LoginViewModel Login { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Check for admin login (hardcoded as per original application)
            var adminEmail = _configuration["AppSettings:AdminEmail"] ?? "admin@gmail.com";
            var adminPassword = _configuration["AppSettings:AdminPassword"] ?? "admin";

            if (Login.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) &&
                Login.Password == adminPassword)
            {
                HttpContext.Session.SetString("UserEmail", Login.Email);
                HttpContext.Session.SetString("UserName", "Administrator");
                HttpContext.Session.SetString("IsAdmin", "true");
                _logger.LogInformation("Admin logged in: {Email}", Login.Email);
                return RedirectToPage("/Admin/Dashboard");
            }

            // Regular user login
            var loginDto = Login.ToLoginDto();
            var user = await _userService.ValidateLoginAsync(loginDto, cancellationToken);

            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("IsAdmin", "false");

            _logger.LogInformation("User logged in: {Email}", user.Email);
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Login.Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
