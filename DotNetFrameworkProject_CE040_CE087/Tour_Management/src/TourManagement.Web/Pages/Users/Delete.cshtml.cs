using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for deleting a user.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets or sets the user view model.</summary>
    [BindProperty]
    public UserViewModel? User { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="DeleteModel"/>.
    /// </summary>
    public DeleteModel(IUserService userService, ILogger<DeleteModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the delete user confirmation page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _userService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
                return NotFound();

            User = new UserViewModel
            {
                Id = dto.Id,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user for delete, id {UserId}", id);
            TempData["ErrorMessage"] = "An error occurred while loading the user.";
            return RedirectToPage("Index");
        }
    }

    /// <summary>
    /// Handles POST requests to delete a user.
    /// </summary>
    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        if (User is null)
            return RedirectToPage("Index");

        try
        {
            await _userService.DeleteAsync(User.Id, cancellationToken);
            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with id {UserId}", User.Id);
            TempData["ErrorMessage"] = "An error occurred while deleting the user.";
            return RedirectToPage("Index");
        }
    }
}
