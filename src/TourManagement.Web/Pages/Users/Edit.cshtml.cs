using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Entities;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

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

    public async Task<IActionResult> OnGetAsync(string email, CancellationToken cancellationToken)
    {
        var sessionEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(sessionEmail))
            return RedirectToPage("./Login");

        // Users can only edit their own profile unless admin
        if (sessionEmail != email && HttpContext.Session.GetString("IsAdmin") != "true")
            return Forbid();

        var user = await _userService.GetUserByEmailAsync(email, cancellationToken);
        if (user == null)
            return NotFound();

        Input = new UserEditViewModel
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Gender = user.Gender,
            Dob = user.Dob,
            Street = user.Street,
            City = user.City,
            State = user.State,
            IsActive = user.IsActive
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var sessionEmail = HttpContext.Session.GetString("UserEmail");
        if (string.IsNullOrEmpty(sessionEmail))
            return RedirectToPage("./Login");

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
                Dob = Input.Dob,
                Street = Input.Street,
                City = Input.City,
                State = Input.State,
                IsActive = Input.IsActive
            };

            await _userService.UpdateUserAsync(user, cancellationToken);
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage("./Profile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user: {Email}", Input.Email);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the profile.");
            return Page();
        }
    }
}
