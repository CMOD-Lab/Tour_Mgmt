using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Application.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Admin;

/// <summary>
/// Page model for admin user management.
/// </summary>
public class UsersModel : PageModel
{
    private readonly UserService _userService;
    private readonly ILogger<UsersModel> _logger;

    public UsersModel(UserService userService, ILogger<UsersModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    public IEnumerable<UserListViewModel> Users { get; set; } = Enumerable.Empty<UserListViewModel>();

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (HttpContext.Session.GetString("IsAdmin") != "true")
            return RedirectToPage("./Login");

        try
        {
            var dtos = await _userService.GetAllAsync(cancellationToken);

            // Manual mapping from DTO to ViewModel
            Users = dtos.Select(dto => new UserListViewModel
            {
                UserId = dto.UserId,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                City = dto.City,
                State = dto.State,
                CreatedDate = dto.CreatedDate
            });
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users for admin");
            Users = Enumerable.Empty<UserListViewModel>();
            return Page();
        }
    }
}
