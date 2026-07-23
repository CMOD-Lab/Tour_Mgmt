using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for deleting a user.
/// </summary>
public class DeleteModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IUserService userService, ILogger<DeleteModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public UserViewModel? User { get; set; }

    public async Task<IActionResult> OnGetAsync(string email)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("Login");

        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound();

            User = new UserViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                City = user.City,
                State = user.State
            };
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading user for delete: {Email}", email);
            return RedirectToPage("Index");
        }
    }

    public async Task<IActionResult> OnPostAsync(string email)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("Login");

        try
        {
            await _userService.DeleteUserAsync(email);
            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {Email}", email);
            TempData["ErrorMessage"] = "An error occurred while deleting the user.";
            return RedirectToPage("Index");
        }
    }
}
