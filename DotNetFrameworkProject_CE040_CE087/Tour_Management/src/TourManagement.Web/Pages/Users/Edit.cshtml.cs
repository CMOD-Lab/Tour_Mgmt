using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for editing a user profile.
/// </summary>
public class EditModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the edit input.</summary>
    [BindProperty]
    public UserEditViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of <see cref="EditModel"/>.
    /// </summary>
    public EditModel(IUserService userService, ILogger<EditModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the edit profile page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var sessionUserId = HttpContext.Session.GetInt32("UserId");
        var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

        if (sessionUserId == null || (sessionUserId != id && !isAdmin))
        {
            TempData["ErrorMessage"] = "You are not authorized to edit this profile.";
            return RedirectToPage("Login");
        }

        try
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToPage("Profile");
            }

            // Map DTO to ViewModel manually
            Input = new UserEditViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Street = user.Street,
                City = user.City,
                State = user.State,
                IsActive = user.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user for edit, id {UserId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the profile.";
            return RedirectToPage("Profile");
        }
    }

    /// <summary>
    /// Handles POST requests to update a user profile.
    /// </summary>
    public async Task<IActionResult> OnPostAsync()
    {
        var sessionUserId = HttpContext.Session.GetInt32("UserId");
        var isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

        if (sessionUserId == null || (sessionUserId != Input.Id && !isAdmin))
        {
            return RedirectToPage("Login");
        }

        if (!ModelState.IsValid)
            return Page();

        try
        {
            // Map ViewModel to DTO manually
            var dto = new UserUpdateDto
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                DateOfBirth = Input.DateOfBirth,
                Street = Input.Street,
                City = Input.City,
                State = Input.State,
                IsActive = Input.IsActive
            };

            await _userService.UpdateAsync(Input.Id, dto);
            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToPage("Profile");
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToPage("Profile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with id {UserId}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the profile. Please try again.");
            return Page();
        }
    }
}
