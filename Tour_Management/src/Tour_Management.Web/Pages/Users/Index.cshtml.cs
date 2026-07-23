using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>
/// Page model for admin user management index.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public IEnumerable<UserViewModel> Users { get; set; } = Enumerable.Empty<UserViewModel>();

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("Login");

        try
        {
            var users = await _userService.GetAllUsersAsync();
            Users = users.Select(u => new UserViewModel
            {
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Gender = u.Gender,
                DateOfBirth = u.DateOfBirth,
                Street = u.Street,
                City = u.City,
                State = u.State
            });
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users list");
            return Page();
        }
    }
}
