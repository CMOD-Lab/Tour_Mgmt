using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.DTOs;
using Tour_Management.Application.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for user registration.
/// </summary>
public class RegisterModel : PageModel
{
    private readonly UserService _userService;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(UserService userService, ILogger<RegisterModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public RegisterViewModel Input { get; set; } = new();

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("UserEmail") != null)
            return RedirectToPage("/Users/Profile");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Manual mapping from ViewModel to DTO
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

            await _userService.RegisterAsync(dto, cancellationToken);
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToPage("./Login");
        }
        catch (Domain.Exceptions.ValidationException ex)
        {
            foreach (var error in ex.Errors)
                ModelState.AddModelError(string.Empty, error);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", Input.Email);
            ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
            return Page();
        }
    }
}
