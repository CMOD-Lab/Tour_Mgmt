using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Entities;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for editing user profile.
/// </summary>
public class EditModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IUserService userService, ILogger<EditModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    public UserEditViewModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string email)
    {
        var sessionEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(sessionEmail))
            return RedirectToPage("Login");

        // Users can only edit their own profile (unless admin)
        if (sessionEmail != email && HttpContext.Session.GetString("IsAdmin") != "true")
            return Forbid();

        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound();

            Input = new UserEditViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Street = user.Street,
                City = user.City,
                State = user.State
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user for edit: {Email}", email);
            return RedirectToPage("Profile");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var sessionEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(sessionEmail))
            return RedirectToPage("Login");

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
                DateOfBirth = Input.DateOfBirth,
                Street = Input.Street,
                City = Input.City,
                State = Input.State
            };

            await _userService.UpdateUserAsync(user);
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage("Profile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {Email}", Input.Email);
            ErrorMessage = "An error occurred while updating your profile. Please try again.";
            return Page();
        }
    }
}
