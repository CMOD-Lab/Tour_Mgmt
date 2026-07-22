using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.DTOs;
using TourManagement.Domain.Exceptions;
using TourManagement.Application.Interfaces;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for deleting a user (admin only).
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<DeleteModel> _logger;

    /// <summary>Gets the user to be deleted.</summary>
    public new UserDto? User { get; private set; }

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
    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            User = await _userService.GetByIdAsync(id);
            if (User == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToPage("Index");
            }
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
    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Admin/Login");
        }

        try
        {
            await _userService.DeleteAsync(id);
            TempData["SuccessMessage"] = "User was deleted successfully.";
            return RedirectToPage("Index");
        }
        catch (NotFoundException)
        {
            TempData["ErrorMessage"] = "User not found.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with id {UserId}", id);
            TempData["ErrorMessage"] = "An error occurred while deleting the user.";
            return RedirectToPage("Index");
        }
    }
}
