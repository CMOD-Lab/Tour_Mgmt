using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management.Pages.Tours
{
    /// <summary>
    /// Razor Page model for Order — replaces Order.aspx Web Forms page.
    /// Uses Entity Framework Core with Azure SQL connection resiliency
    /// instead of direct SqlConnection management (cr-dotnet-0013).
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Connection string read from environment-based configuration (cr-dotnet-0010).
    /// </summary>
    public class OrderModel : PageModel
    {
        private readonly TourManagementDbContext _dbContext;
        private readonly ILogger<OrderModel> _logger;

        public OrderModel(TourManagementDbContext dbContext, ILogger<OrderModel> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string StatusMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }

        public class InputModel
        {
            [Required]
            [MaxLength(200)]
            [Display(Name = "Tour Name")]
            public string TourName { get; set; } = string.Empty;

            [Required]
            [MaxLength(200)]
            public string City { get; set; } = string.Empty;

            [Required]
            [MaxLength(200)]
            [EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required]
            [MaxLength(200)]
            public string Name { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Please correct the errors below.";
                IsSuccess = false;
                return Page();
            }

            try
            {
                // Use EF Core DbContext — built-in connection pooling and
                // Azure SQL transient fault handling via EnableRetryOnFailure
                var booking = new Booking
                {
                    TourName = Input.TourName,
                    Place = Input.City,
                    Email = Input.Email,
                    FirstName = Input.Name
                };

                _dbContext.Bookings.Add(booking);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Booking registered for tour '{TourName}' by '{Email}'.",
                    Input.TourName, Input.Email);

                // Stateless redirect — no server affinity required
                return RedirectToPage("/User/MyBooking");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering booking for tour '{TourName}'.", Input.TourName);
                StatusMessage = "An error occurred while processing your booking. Please try again.";
                IsSuccess = false;
                return Page();
            }
        }
    }
}
