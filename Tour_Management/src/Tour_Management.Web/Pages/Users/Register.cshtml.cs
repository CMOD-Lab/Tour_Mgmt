using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Exceptions;
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

    public RegisterModel(IUserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public UserRegisterViewModel Input { get; set; } = new();

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
            var user = new UserInfo
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

            await _userService.RegisterUserAsync(user);
            _logger.LogInformation("New user registered: {Email}", Input.Email);
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToPage("Login");
        }
        catch (DuplicateEntityException)
        {
            ErrorMessage = "An account with this email already exists. Please use a different email or login.";
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", Input.Email);
            ErrorMessage = "An error occurred during registration. Please try again.";
            return Page();
        }
    }
}
