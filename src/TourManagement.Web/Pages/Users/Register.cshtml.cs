using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>Page model for the user registration page.</summary>
public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<RegisterModel> _logger;

    /// <summary>Gets or sets the registration input model.</summary>
    [BindProperty]
    public RegisterViewModel Input { get; set; } = new();

    /// <summary>Gets or sets the success message.</summary>
    public string SuccessMessage { get; set; } = string.Empty;

    /// <summary>Gets or sets the error message.</summary>
    public string ErrorMessage { get; set; } = string.Empty;

    /// <summary>Initializes a new instance of <see cref="RegisterModel"/>.</summary>
    public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the registration page.</summary>
    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
        {
            return RedirectToPage("/Users/Profile");
        }
        return Page();
    }

    /// <summary>Handles POST requests for the registration form submission.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

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

            var result = await _userService.CreateAsync(dto, cancellationToken);
            if (!result)
            {
                ErrorMessage = "An account with this email already exists.";
                return Page();
            }

            _logger.LogInformation("New user registered with email {Email}", Input.Email);
            return RedirectToPage("/Users/Login", new { message = "Registration successful! Please login." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email {Email}", Input.Email);
            ErrorMessage = "An error occurred during registration. Please try again.";
            return Page();
        }
    }
}
