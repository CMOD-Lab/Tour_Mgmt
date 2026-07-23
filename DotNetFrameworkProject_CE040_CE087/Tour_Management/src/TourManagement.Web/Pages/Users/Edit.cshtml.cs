using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Exceptions;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for editing a user profile.
/// </summary>
public class EditModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the input view model.</summary>
    [BindProperty]
    public UserEditViewModel Input { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EditModel"/> class.
    /// </summary>
    public EditModel(IUserService userService, ILogger<EditModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the edit profile page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id, cancellationToken);
            if (user == null)
                return NotFound();

            // Map Entity to ViewModel manually
            Input = new UserEditViewModel
            {
                Id = user.Id,
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
            _logger.LogError(ex, "Error loading edit page for user id {UserId}", id);
            return RedirectToPage("Login");
        }
    }

    /// <summary>Handles POST requests to update a user profile.</summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var existingUser = await _userService.GetByIdAsync(Input.Id, cancellationToken);
            if (existingUser == null)
                return NotFound();

            // Update entity properties
            existingUser.FirstName = Input.FirstName;
            existingUser.LastName = Input.LastName;
            existingUser.Gender = Input.Gender;
            existingUser.DateOfBirth = Input.DateOfBirth;
            existingUser.Street = Input.Street;
            existingUser.City = Input.City;
            existingUser.State = Input.State;
            existingUser.ModifiedBy = User.Identity?.Name ?? "system";

            await _userService.UpdateAsync(existingUser, cancellationToken);
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage("Profile", new { id = Input.Id });
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with id {UserId}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the profile. Please try again.");
            return Page();
        }
    }
}
