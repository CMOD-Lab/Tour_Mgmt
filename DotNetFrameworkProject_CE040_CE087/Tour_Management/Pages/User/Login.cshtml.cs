using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tour_Management.Data;

namespace Tour_Management.Pages.User
{
    /// <summary>
    /// Razor Page model for Login — replaces userlogin.aspx Web Forms page.
    /// Uses Entity Framework Core with Azure SQL connection resiliency
    /// instead of direct SqlConnection management (cr-dotnet-0013).
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Connection string read from environment-based configuration (cr-dotnet-0010).
    /// </summary>
    public class LoginModel : PageModel
    {
        private readonly TourManagementDbContext _dbContext;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(TourManagementDbContext dbContext, ILogger<LoginModel> logger)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Please enter your email and password.";
                IsSuccess = false;
                return Page();
            }

            try
            {
                // Use EF Core DbContext — built-in connection pooling and
                // Azure SQL transient fault handling via EnableRetryOnFailure.
                // Replaces direct SqlConnection with parameterized query pattern.
                var user = await _dbContext.UserInfos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u =>
                        u.Email == Input.Email &&
                        u.Password == Input.Password);

                if (user != null)
                {
                    _logger.LogInformation("User '{Email}' logged in successfully.", Input.Email);
                    StatusMessage = "Password is correct";
                    IsSuccess = true;
                    // Stateless redirect — no server affinity required
                    return RedirectToPage("/User/MainProfile");
                }
                else
                {
                    _logger.LogWarning("Failed login attempt for '{Email}'.", Input.Email);
                    StatusMessage = "Password is not correct";
                    IsSuccess = false;
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for '{Email}'.", Input.Email);
                StatusMessage = "An error occurred during login. Please try again.";
                IsSuccess = false;
                return Page();
            }
        }
    }
}
