using System.ComponentModel.DataAnnotations;

namespace TourBooking.Web.ViewModels;

/// <summary>
/// ViewModel for user login page.
/// </summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel for user registration page.
/// </summary>
public class RegisterViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(50)]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required")]
    [Display(Name = "Gender")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime Dob { get; set; }

    [Required(ErrorMessage = "Street is required")]
    [StringLength(50)]
    [Display(Name = "Street")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required")]
    [StringLength(50)]
    [Display(Name = "City")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required")]
    [StringLength(50)]
    [Display(Name = "State")]
    public string State { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel for displaying user profile.
/// </summary>
public class UserProfileViewModel
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime Dob { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}
