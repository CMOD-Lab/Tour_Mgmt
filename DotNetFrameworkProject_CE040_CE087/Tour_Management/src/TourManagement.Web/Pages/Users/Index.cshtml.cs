using Microsoft.AspNetCore.Mvc.RazorPages;
using TourManagement.Application.Interfaces;
using TourManagement.Web.ViewModels;

namespace TourManagement.Web.Pages.Users;

/// <summary>
/// Page model for the users list page.
/// </summary>
public class IndexModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<IndexModel> _logger;

    /// <summary>Gets or sets the list of users.</summary>
    public IEnumerable<UserViewModel> Users { get; set; } = Enumerable.Empty<UserViewModel>();

    /// <summary>Gets or sets the search term.</summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="IndexModel"/>.
    /// </summary>
    public IndexModel(IUserService userService, ILogger<IndexModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Handles GET requests for the users list page.
    /// </summary>
    public async Task OnGetAsync(string? searchTerm = null, CancellationToken cancellationToken = default)
    {
        SearchTerm = searchTerm;
        try
        {
            var dtos = string.IsNullOrWhiteSpace(searchTerm)
                ? await _userService.GetAllAsync(cancellationToken)
                : await _userService.SearchAsync(searchTerm, cancellationToken);

            Users = dtos.Select(dto => new UserViewModel
            {
                Id = dto.Id,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Dob = dto.Dob,
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                Role = dto.Role,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users list");
            TempData["ErrorMessage"] = "An error occurred while loading users.";
        }
    }
}
