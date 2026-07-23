using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for editing a user.
/// </summary>
public class EditModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<EditModel> _logger;

    /// <summary>Gets or sets the user edit input model.</summary>
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
    /// Handles GET requests for the edit user page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _userService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
                return NotFound();

            Input = new UserEditViewModel
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Dob = dto.Dob,
                Street = dto.Street,
                City = dto.City,
                State = dto.State
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user for edit, id {UserId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the user.";
            return RedirectToPage("Index");
        }
    }

    /// <summary>
    /// Handles POST requests to update a user.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var dto = new UserUpdateDto
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                Dob = Input.Dob,
                Street = Input.Street,
                City = Input.City,
                State = Input.State,
                ModifiedBy = HttpContext.Session.GetString("UserEmail") ?? "system"
            };

            await _userService.UpdateAsync(Input.Id, dto, cancellationToken);
            TempData["SuccessMessage"] = "User updated successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with id {UserId}", Input.Id);
            TempData["ErrorMessage"] = "An error occurred while updating the user.";
            return Page();
        }
    }
}
