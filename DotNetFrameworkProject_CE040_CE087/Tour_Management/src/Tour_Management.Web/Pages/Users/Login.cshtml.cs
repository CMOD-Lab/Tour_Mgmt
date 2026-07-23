using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for user login.
/// </summary>
public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginModel"/> class.
    /// </summary>
    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Gets or sets the login view model.</summary>
    [BindProperty]
    public UserLoginViewModel Login { get; set; } = new();

    /// <summary>Gets or sets the success message.</summary>
    public string? Message { get; set; }

    /// <summary>
    /// Handles GET requests for the login page.
    /// </summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            return RedirectToPage("/Index");
        }
        Message = TempData["Message"]?.ToString();
        return Page();
    }

    /// <summary>
    /// Handles POST requests to authenticate a user.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var user = await _userService.ValidateCredentialsAsync(Login.Email, Login.Password, cancellationToken);
            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                HttpContext.Session.SetInt32("UserId", user.UserId);
                _logger.LogInformation("User logged in: {Email}", user.Email);
                return RedirectToPage("/Tours/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", Login.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during login.");
            return Page();
        }
    }
}
