using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Domain.DTOs;
using TourManagement.Domain.Interfaces.Services;

namespace TourManagement.Web.Pages.User;

/// <summary>
/// User management index page model - migrated from usercrud.aspx.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    public IEnumerable<UserDto> Users { get; set; } = Enumerable.Empty<UserDto>();
    public string? SearchTerm { get; set; }
    public string? SuccessMessage { get; set; }

    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync(string? searchTerm, CancellationToken cancellationToken)
    {
        // Admin only
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("/User/Login");
        }

        SearchTerm = searchTerm;

        try
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                Users = await _userService.SearchAsync(searchTerm, cancellationToken);
            }
            else
            {
                Users = await _userService.GetAllAsync(cancellationToken);
            }
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
            return RedirectToPage("/User/Login");
        }

        try
        {
            await _userService.DeleteAsync(email, cancellationToken);
            _logger.LogInformation("User deleted by admin: {Email}", email);
            TempData["SuccessMessage"] = $"User {email} deleted successfully.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user: {Email}", email);
        }

        return RedirectToPage();
    }
}
