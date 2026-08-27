using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for editing user profile.
/// </summary>
[Authorize]
public class EditModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the user edit view model.</summary>
    [BindProperty]
    public UserEditViewModel Input { get; set; } = new();

    /// <summary>Initializes a new instance of EditModel.</summary>
    public EditModel(IUserService userService, ILogger<EditModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the edit profile page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return RedirectToPage("Login");

            var dto = await _userService.GetByEmailAsync(email, cancellationToken);
            if (dto == null)
                return NotFound();

            // Manual mapping from DTO to ViewModel
            Input = new UserEditViewModel
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Street = dto.Street,
                City = dto.City,
                State = dto.State
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user profile for edit");
            return RedirectToPage("Profile");
        }
    }

    /// <summary>Handles POST requests to update user profile.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var email = User.Identity?.Name;
            if (string.IsNullOrEmpty(email))
                return RedirectToPage("Login");

            // Manual mapping from ViewModel to DTO
            var updateDto = new UserUpdateDto
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                Street = Input.Street,
                City = Input.City,
                State = Input.State,
                IsActive = true
            };

            await _userService.UpdateAsync(email, updateDto, cancellationToken);
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage("Profile");
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile");
            ModelState.AddModelError(string.Empty, "An error occurred while updating your profile.");
            return Page();
        }
    }
}
