using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management.Pages.Tours
{
    /// <summary>
    /// Razor Page model for DisplayTours — replaces DisplayTours.aspx Web Forms page.
    /// Uses Entity Framework Core with Azure SQL connection resiliency (cr-dotnet-0013).
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Connection string read from environment-based configuration (cr-dotnet-0010).
    /// </summary>
    public class DisplayToursModel : PageModel
    {
        private readonly TourManagementDbContext _dbContext;
        private readonly ILogger<DisplayToursModel> _logger;

        public DisplayToursModel(TourManagementDbContext dbContext, ILogger<DisplayToursModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IList<Tour> Tours { get; set; } = new List<Tour>();

        public async Task OnGetAsync()
        {
            try
            {
                // EF Core query with built-in connection pooling and Azure SQL resiliency
                Tours = await _dbContext.Tours
                    .AsNoTracking()
                    .OrderBy(t => t.TourName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading tours.");
                Tours = new List<Tour>();
            }
        }
    }
}
