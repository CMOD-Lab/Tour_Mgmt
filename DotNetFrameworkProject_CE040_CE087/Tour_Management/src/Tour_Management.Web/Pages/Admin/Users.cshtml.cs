using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin;

/// <summary>
/// Page model for managing users in the admin panel.
/// </summary>
public class UsersModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersModel> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersModel"/> class.
    /// </summary>
    public UsersModel(IUserService userService, ILogger<UsersModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>Gets the list of users.</summary>
    public IEnumerable<UserIndexViewModel> Users { get; private set; } = Enumerable.Empty<UserIndexViewModel>();

    /// <summary>
    /// Handles GET requests for the admin users page.
    /// </summary>
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
        {
            return RedirectToPage("./Login");
        }

        try
        {
            var users = await _userService.GetAllAsync(cancellationToken);

            // Manual ViewModel mapping
            Users = users.Select(u => new UserIndexViewModel
            {
                UserId = u.UserId,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.IsActive,
                CreatedDate = u.CreatedDate
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
