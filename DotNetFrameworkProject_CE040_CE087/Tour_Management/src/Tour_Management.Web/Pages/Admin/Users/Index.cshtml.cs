using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Interfaces;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin.Users;

/// <summary>
/// Page model for the admin users listing page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets or sets the list of users.</summary>
    public IEnumerable<UserIndexViewModel> Users { get; set; } = Enumerable.Empty<UserIndexViewModel>();

    /// <summary>
    /// Initializes a new instance of the <see cref="IndexModel"/> class.
    /// </summary>
    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Handles GET requests for the admin users listing page.</summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("/Admin/Login");

        try
        {
            var dtos = await _userService.GetAllAsync(cancellationToken);
            Users = dtos.Select(u => new UserIndexViewModel
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                City = u.City,
                IsActive = u.IsActive
            });
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading admin users list");
            TempData["Error"] = "An error occurred while loading users.";
            return Page();
        }
    }
}
