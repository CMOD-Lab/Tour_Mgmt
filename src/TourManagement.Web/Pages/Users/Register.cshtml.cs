using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for the user registration page.
/// </summary>
public class RegisterModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<RegisterModel> _logger;

    /// <summary>Gets or sets the registration input view model.</summary>
    [BindProperty]
    public RegisterViewModel Input { get; set; } = new();

    /// <summary>Initializes a new instance of RegisterModel.</summary>
    public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the registration page.</summary>
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");
        return Page();
    }

    /// <summary>Handles POST requests for user registration.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Manual mapping from ViewModel to DTO
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

            await _userService.RegisterAsync(createDto, cancellationToken);
            _logger.LogInformation("New user registered: {Email}", Input.Email);
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToPage("Login");
        }
        catch (DuplicateEntityException)
        {
            ModelState.AddModelError("Input.Email", "An account with this email already exists.");
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", Input.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during registration.");
            return Page();
        }
    }
}
