using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management.Pages.Admin
{
    /// <summary>
    /// Razor Page model for AllBooking — replaces allbooking.aspx Web Forms page.
    /// Uses Entity Framework Core with Azure SQL connection resiliency (cr-dotnet-0013).
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Connection string read from environment-based configuration (cr-dotnet-0010).
    /// </summary>
    public class AllBookingModel : PageModel
    {
        private readonly TourManagementDbContext _dbContext;
        private readonly ILogger<AllBookingModel> _logger;

        public AllBookingModel(TourManagementDbContext dbContext, ILogger<AllBookingModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IList<Booking> Bookings { get; set; } = new List<Booking>();

        public async Task OnGetAsync()
        {
            try
            {
                // EF Core query with built-in connection pooling and Azure SQL resiliency
                Bookings = await _dbContext.Bookings
                    .AsNoTracking()
                    .OrderBy(b => b.TourName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading all bookings.");
                Bookings = new List<Booking>();
            }
        }
    }
}
