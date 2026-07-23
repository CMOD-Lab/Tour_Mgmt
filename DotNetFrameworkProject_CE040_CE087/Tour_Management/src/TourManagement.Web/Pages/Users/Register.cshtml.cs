using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for user registration.
/// </summary>
public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<RegisterModel> _logger;

    /// <summary>Gets or sets the registration input model.</summary>
    [BindProperty]
    public UserRegisterViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="RegisterModel"/>.
    /// </summary>
    public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the registration page.
    /// </summary>
    public void OnGet()
    {
    }

    /// <summary>
    /// Handles POST requests to register a new user.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var dto = new UserCreateDto
            {
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                Password = Input.Password,
                Dob = Input.Dob,
                Street = Input.Street,
                City = Input.City,
                State = Input.State
            };

            await _userService.CreateAsync(dto, cancellationToken);
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToPage("Login");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", Input.Email);
            TempData["ErrorMessage"] = "An error occurred during registration. The email may already be in use.";
            return Page();
        }
    }
}
