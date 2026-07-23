using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin.Users;

/// <summary>
/// Page model for deleting a user (admin).
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets or sets the user to delete.</summary>
    public UserDeleteViewModel? UserToDelete { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteModel"/> class.
    /// </summary>
    public DeleteModel(IUserService userService, ILogger<DeleteModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the user delete confirmation page.</summary>
    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        var dto = await _userService.GetByIdAsync(id, cancellationToken);
        if (dto == null)
            return NotFound();

        UserToDelete = new UserDeleteViewModel
        {
            UserId = dto.UserId,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };
        return Page();
    }

    /// <summary>Handles POST requests for the user deletion.</summary>
    public async Task<IActionResult> OnPostAsync(int id, CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        try
        {
            await _userService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Admin deleted user id {UserId}", id);
            TempData["Success"] = "User deleted successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user id {UserId}", id);
            TempData["Error"] = "An error occurred while deleting the user.";
            return RedirectToPage("./Index");
        }
    }
}
