using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>Page model for editing a user profile.</summary>
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
    public new UserEditViewModel User { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
        {
            return RedirectToPage("./Login");
        }

        try
        {
            var dto = await _userService.GetByIdAsync(id, cancellationToken);
            if (dto == null) return NotFound();

            User = UserEditViewModel.FromDto(dto);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user for edit, ID {UserId}", id);
            return RedirectToPage("./Profile");
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail")))
        {
            return RedirectToPage("./Login");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var updateDto = User.ToUpdateDto();
            var result = await _userService.UpdateAsync(User.UserId, updateDto, cancellationToken);

            if (result == null) return NotFound();

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToPage("./Profile");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID {UserId}", User.UserId);
            ModelState.AddModelError(string.Empty, "An error occurred while updating your profile. Please try again.");
            return Page();
        }
    }
}
