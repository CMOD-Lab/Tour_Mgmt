using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management.Pages.User
{
    /// <summary>
    /// Razor Page model for MyBooking — replaces mybooking.aspx Web Forms page.
    /// Uses Entity Framework Core with Azure SQL connection resiliency (cr-dotnet-0013).
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Connection string read from environment-based configuration (cr-dotnet-0010).
    /// </summary>
    public class MyBookingModel : PageModel
    {
        private readonly TourManagementDbContext _dbContext;
        private readonly ILogger<MyBookingModel> _logger;

        public MyBookingModel(TourManagementDbContext dbContext, ILogger<MyBookingModel> logger)
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
                _logger.LogError(ex, "Error loading bookings.");
                Bookings = new List<Booking>();
            }
        }
    }
}
