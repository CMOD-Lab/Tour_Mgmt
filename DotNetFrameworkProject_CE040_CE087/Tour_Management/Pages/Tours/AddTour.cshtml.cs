using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management.Pages.Tours
{
    /// <summary>
    /// Razor Page model for AddTour — replaces AddTour.aspx Web Forms page.
    /// Uses Entity Framework Core with Azure SQL connection resiliency
    /// instead of direct SqlConnection management (cr-dotnet-0013).
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Connection string read from environment-based configuration (cr-dotnet-0010).
    /// </summary>
    public class AddTourModel : PageModel
    {
        private readonly TourManagementDbContext _dbContext;
        private readonly ILogger<AddTourModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public AddTourModel(
            TourManagementDbContext dbContext,
            ILogger<AddTourModel> logger,
            IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _logger = logger;
            _environment = environment;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string StatusMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }

        public class InputModel
        {
            [Required]
            [MaxLength(200)]
            [Display(Name = "Name of Tour")]
            public string TourName { get; set; } = string.Empty;

            [Required]
            [MaxLength(200)]
            public string Place { get; set; } = string.Empty;

            [Required]
            [Range(1, 365)]
            public int Days { get; set; }

            [Required]
            [Range(0.01, double.MaxValue)]
            public decimal Price { get; set; }

            [MaxLength(500)]
            public string? Locations { get; set; }

            [MaxLength(250)]
            [Display(Name = "Tour Info")]
            public string? TourInfo { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? TourImage)
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Please correct the errors below.";
                IsSuccess = false;
                return Page();
            }

            try
            {
                string? picFileName = null;

                // Handle file upload — store filename reference in database
                if (TourImage != null && TourImage.Length > 0)
                {
                    var tourPicsPath = Path.Combine(_environment.WebRootPath, "Tour_pics");
                    if (!Directory.Exists(tourPicsPath))
                        Directory.CreateDirectory(tourPicsPath);

                    picFileName = TourImage.FileName;
                    var filePath = Path.Combine(tourPicsPath, picFileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await TourImage.CopyToAsync(stream);
                }

                // Use EF Core DbContext — built-in connection pooling and
                // Azure SQL transient fault handling via EnableRetryOnFailure
                var tour = new Tour
                {
                    TourName = Input.TourName,
                    Place = Input.Place,
                    Days = Input.Days,
                    Price = Input.Price,
                    Locations = Input.Locations,
                    TourInfo = Input.TourInfo,
                    Pic = picFileName
                };

                _dbContext.Tours.Add(tour);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Tour '{TourName}' added successfully.", Input.TourName);
                StatusMessage = "ADD Successful";
                IsSuccess = true;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding tour '{TourName}'.", Input.TourName);
                StatusMessage = "An error occurred while adding the tour. Please try again.";
                IsSuccess = false;
                return Page();
            }
        }
    }
}
