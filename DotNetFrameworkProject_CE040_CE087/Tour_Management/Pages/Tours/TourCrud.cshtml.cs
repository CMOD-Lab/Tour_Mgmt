using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management.Pages.Tours
{
    /// <summary>
    /// Razor Page model for TourCrud — replaces TourCrud.aspx Web Forms page.
    /// Uses Entity Framework Core with Azure SQL connection resiliency
    /// instead of direct SqlConnection management (cr-dotnet-0013).
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Connection string read from environment-based configuration (cr-dotnet-0010).
    /// </summary>
    public class TourCrudModel : PageModel
    {
        private readonly TourManagementDbContext _dbContext;
        private readonly ILogger<TourCrudModel> _logger;

        public TourCrudModel(TourManagementDbContext dbContext, ILogger<TourCrudModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IList<Tour> Tours { get; set; } = new List<Tour>();
        public string StatusMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }

        public async Task OnGetAsync()
        {
            await RefreshDataAsync();
        }

        /// <summary>
        /// Replaces the refreshdata() method that used direct SqlConnection.
        /// EF Core handles connection pooling and Azure SQL resiliency automatically.
        /// </summary>
        private async Task RefreshDataAsync()
        {
            try
            {
                Tours = await _dbContext.Tours
                    .AsNoTracking()
                    .OrderBy(t => t.TourName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading tours for CRUD.");
                Tours = new List<Tour>();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var tour = await _dbContext.Tours.FindAsync(id);
                if (tour != null)
                {
                    _dbContext.Tours.Remove(tour);
                    await _dbContext.SaveChangesAsync();
                    _logger.LogInformation("Tour with ID {TourId} deleted.", id);
                    StatusMessage = "Tour deleted successfully.";
                    IsSuccess = true;
                }
                else
                {
                    StatusMessage = "Tour not found.";
                    IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tour with ID {TourId}.", id);
                StatusMessage = "An error occurred while deleting the tour.";
                IsSuccess = false;
            }

            await RefreshDataAsync();
            return Page();
        }
    }
}
