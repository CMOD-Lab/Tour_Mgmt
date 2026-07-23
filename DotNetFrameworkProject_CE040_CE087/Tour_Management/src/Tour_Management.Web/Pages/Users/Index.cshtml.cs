using Microsoft.AspNetCore.Mvc.RazorPages;
using Tour_Management.Domain.Interfaces.Services;
using Tour_Management.Web.ViewModels;

namespace Tour_Management.Web.Pages.Users;

/// <summary>Page model for the users index/list page.</summary>
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

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var dtos = await _userService.GetAllAsync(cancellationToken);

            // Manual mapping from DTO to ViewModel
            Users = dtos.Select(dto => new UserViewModel
            {
                UserId = dto.UserId,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Dob = dto.Dob,
                Street = dto.Street,
                City = dto.City,
                State = dto.State,
                CreatedDate = dto.CreatedDate,
                IsActive = dto.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading users index page");
            Users = Enumerable.Empty<UserViewModel>();
        }
    }
}
