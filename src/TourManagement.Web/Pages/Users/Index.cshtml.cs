using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.Interfaces.Services;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for user management list (admin).
/// </summary>
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    public IEnumerable<UserListViewModel> Users { get; set; } = Enumerable.Empty<UserListViewModel>();

    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            var users = await _userService.GetAllAsync(cancellationToken);
            // Manually map domain entities to ViewModels
            Users = users.Select(u => new UserListViewModel
            {
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Gender = u.Gender,
                City = u.City,
                State = u.State
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users list");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string email, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/Users/Login");
        }

        try
        {
            await _userService.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("User {Email} deleted by admin", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user {Email}", email);
        }

        return RedirectToPage();
    }
}
