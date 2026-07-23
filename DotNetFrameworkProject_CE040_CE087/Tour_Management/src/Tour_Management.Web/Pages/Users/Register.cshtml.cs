using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for user registration.
/// </summary>
public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<RegisterModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterModel"/> class.
    /// </summary>
    public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Gets or sets the user register view model.</summary>
    [BindProperty]
    public UserRegisterViewModel RegisterUser { get; set; } = new();

    /// <summary>
    /// Handles GET requests for the registration page.
    /// </summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }

    /// <summary>
    /// Handles POST requests to register a new user.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Check if email already exists
            var existingUser = await _userService.GetByEmailAsync(RegisterUser.Email, cancellationToken);
            if (existingUser != null)
            {
                ModelState.AddModelError("RegisterUser.Email", "An account with this email already exists.");
                return Page();
            }

            // Manual mapping from ViewModel to Domain entity
            var user = new UserInfo
            {
                Email = RegisterUser.Email,
                FirstName = RegisterUser.FirstName,
                LastName = RegisterUser.LastName,
                Gender = RegisterUser.Gender,
                PasswordHash = RegisterUser.Password, // Will be hashed in service
                Dob = RegisterUser.Dob,
                Street = RegisterUser.Street,
                City = RegisterUser.City,
                State = RegisterUser.State,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "self"
            };

            await _userService.CreateAsync(user, cancellationToken);
            _logger.LogInformation("User registered: {Email}", user.Email);

            TempData["Message"] = "Registration successful! Please log in.";
            return RedirectToPage("./Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", RegisterUser.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during registration.");
            return Page();
        }
    }
}
