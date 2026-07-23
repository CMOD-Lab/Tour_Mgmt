using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>Page model for user login.</summary>
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

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Check admin credentials first
            var adminEmail = _configuration["AdminCredentials:Email"] ?? "admin@gmail.com";
            var adminPassword = _configuration["AdminCredentials:Password"] ?? "admin";

            if (Input.Email == adminEmail && Input.Password == adminPassword)
            {
                HttpContext.Session.SetString("UserEmail", Input.Email);
                HttpContext.Session.SetString("IsAdmin", "true");
                _logger.LogInformation("Admin logged in: {Email}", Input.Email);
                return RedirectToPage("/Admin/Index");
            }

            // Validate regular user credentials
            var user = await _userService.ValidateLoginAsync(Input.Email, Input.Password, cancellationToken);
            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
            HttpContext.Session.Remove("IsAdmin");

            _logger.LogInformation("User logged in: {Email}", Input.Email);
            return RedirectToPage("/Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Input.Email);
            ErrorMessage = "An error occurred during login. Please try again.";
            return Page();
        }
    }
}
