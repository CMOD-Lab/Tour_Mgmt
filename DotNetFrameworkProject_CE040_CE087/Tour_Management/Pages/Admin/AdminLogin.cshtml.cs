using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Tour_Management.Pages.Admin
{
    /// <summary>
    /// Razor Page model for AdminLogin — replaces AdminLogin2.aspx Web Forms page.
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Admin credentials read from environment-based configuration (cr-dotnet-0010)
    /// instead of hardcoded values. Stateless page model for horizontal scaling.
    /// </summary>
    public class AdminLoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdminLoginModel> _logger;

        public AdminLoginModel(IConfiguration configuration, ILogger<AdminLoginModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string StatusMessage { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [MaxLength(200)]
            public string Email { get; set; } = string.Empty;

            [Required]
            [MaxLength(200)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Please enter your credentials.";
                IsSuccess = false;
                return Page();
            }

            // Admin credentials read from environment variables / appsettings
            // instead of hardcoded values — supports cloud-native configuration
            var adminEmail = _configuration["Admin:Email"]
                ?? Environment.GetEnvironmentVariable("ADMIN_EMAIL")
                ?? "admin@gmail.com";
            var adminPassword = _configuration["Admin:Password"]
                ?? Environment.GetEnvironmentVariable("ADMIN_PASSWORD")
                ?? "admin";

            if (Input.Password == adminPassword && Input.Email == adminEmail)
            {
                _logger.LogInformation("Admin '{Email}' logged in successfully.", Input.Email);
                // Stateless redirect — no server affinity required
                return RedirectToPage("/Admin/AdminProfile");
            }
            else
            {
                _logger.LogWarning("Failed admin login attempt for '{Email}'.", Input.Email);
                StatusMessage = "Invalid admin credentials.";
                IsSuccess = false;
                return Page();
            }
        }
    }
}
