using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Tour_Management.Data;
using Tour_Management.Models;

namespace Tour_Management.Pages.User
{
    /// <summary>
    /// Razor Page model for SignUp — replaces SignUpForm.aspx Web Forms page.
    /// Uses Entity Framework Core with Azure SQL connection resiliency
    /// instead of direct SqlConnection management (cr-dotnet-0013).
    /// Eliminates ViewState, postbacks, and server affinity (cr-dotnet-0026).
    /// Connection string read from environment-based configuration (cr-dotnet-0010).
    /// </summary>
    public class SignUpModel : PageModel
    {
        private readonly TourManagementDbContext _dbContext;
        private readonly ILogger<SignUpModel> _logger;

        public SignUpModel(TourManagementDbContext dbContext, ILogger<SignUpModel> logger)
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
            [MaxLength(100)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; } = string.Empty;

            [MaxLength(100)]
            [Display(Name = "Last Name")]
            public string? LastName { get; set; }

            [MaxLength(20)]
            public string? Gender { get; set; }

            [Required]
            [MaxLength(200)]
            [DataType(DataType.Password)]
            public string Password { get; set; } = string.Empty;

            [MaxLength(50)]
            [Display(Name = "Date of Birth")]
            public string? Dob { get; set; }

            [MaxLength(300)]
            public string? Street { get; set; }

            [MaxLength(100)]
            public string? City { get; set; }

            [MaxLength(100)]
            public string? State { get; set; }
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
                var user = new UserInfo
                {
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Gender = Input.Gender,
                    Password = Input.Password,
                    Dob = Input.Dob,
                    Street = Input.Street,
                    City = Input.City,
                    State = Input.State
                };

                _dbContext.UserInfos.Add(user);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("User '{Email}' registered successfully.", Input.Email);

                // Stateless redirect — no server affinity required
                return RedirectToPage("/User/Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user '{Email}'.", Input.Email);
                StatusMessage = "An error occurred during registration. Please try again.";
                IsSuccess = false;
                return Page();
            }
        }
    }
}
