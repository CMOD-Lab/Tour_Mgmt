using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management.Pages.User
{
    /// <summary>
    /// Razor Page model for UserCrud — replaces usercrud.aspx Web Forms page.
    /// Uses Entity Framework Core with Azure SQL connection resiliency (cr-dotnet-0013).
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Connection string read from environment-based configuration (cr-dotnet-0010).
    /// </summary>
    public class UserCrudModel : PageModel
    {
        private readonly TourManagementDbContext _dbContext;
        private readonly ILogger<UserCrudModel> _logger;

        public UserCrudModel(TourManagementDbContext dbContext, ILogger<UserCrudModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IList<UserInfo> Users { get; set; } = new List<UserInfo>();
        public string StatusMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // EF Core query with built-in connection pooling and Azure SQL resiliency
                Users = await _dbContext.UserInfos
                    .AsNoTracking()
                    .OrderBy(u => u.Email)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading users.");
                Users = new List<UserInfo>();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var user = await _dbContext.UserInfos.FindAsync(id);
                if (user != null)
                {
                    _dbContext.UserInfos.Remove(user);
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation("User with ID {UserId} deleted.", id);
                    StatusMessage = "User deleted successfully.";
                    IsSuccess = true;
                }
                else
                {
                    StatusMessage = "User not found.";
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}.", id);
                StatusMessage = "An error occurred while deleting the user.";
                IsSuccess = false;
            }

            await OnGetAsync();
            return Page();
        }
    }
}
