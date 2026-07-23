using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for the user registration page.
/// </summary>
public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<RegisterModel> _logger;

    /// <summary>Gets or sets the registration input model.</summary>
    [BindProperty]
    public UserRegisterViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterModel"/> class.
    /// </summary>
    public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the registration page.</summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
            return RedirectToPage("/Users/Profile");
        return Page();
    }

    /// <summary>Handles POST requests for the registration form submission.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Check if email already exists
            var existingUser = await _userService.GetByEmailAsync(Input.Email, cancellationToken);
            if (existingUser != null)
            {
                ModelState.AddModelError("Input.Email", "An account with this email already exists.");
                return Page();
            }

            var createDto = new UserCreateDto
            {
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                Password = Input.Password,
                DateOfBirth = Input.DateOfBirth,
                Street = Input.Street,
                City = Input.City,
                State = Input.State
            };

            await _userService.CreateAsync(createDto, cancellationToken);
            _logger.LogInformation("New user registered: {Email}", Input.Email);
            TempData["Success"] = "Registration successful! Please login.";
            return RedirectToPage("./Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email {Email}", Input.Email);
            TempData["Error"] = "An error occurred during registration. Please try again.";
            return Page();
        }
    }
}
