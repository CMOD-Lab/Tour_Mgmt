using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for deleting a user (admin only).
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

    [BindProperty]
    public new UserViewModel? User { get; set; }

    public async Task<IActionResult> OnGetAsync(string email, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        var userEntity = await _userService.GetUserByEmailAsync(email, cancellationToken);
        if (userEntity == null)
            return NotFound();

        User = new UserViewModel
        {
            Email = userEntity.Email,
            FirstName = userEntity.FirstName,
            LastName = userEntity.LastName,
            Gender = userEntity.Gender,
            Dob = userEntity.Dob,
            Street = userEntity.Street,
            City = userEntity.City,
            State = userEntity.State
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Users/Login");

        if (User == null)
            return NotFound();

        try
        {
            await _userService.DeleteUserAsync(User.Email, cancellationToken);
            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {Email}", User.Email);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the user.");
            return Page();
        }
    }
}
