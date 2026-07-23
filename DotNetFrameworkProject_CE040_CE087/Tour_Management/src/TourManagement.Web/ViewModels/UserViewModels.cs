using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

/// <summary>
/// ViewModel for displaying a user.
/// </summary>
public class UserViewModel
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public string? Dob { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string Role { get; set; } = "User";
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// ViewModel for user registration.
/// </summary>
public class UserRegisterViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Gender cannot exceed 20 characters")]
    [Display(Name = "Gender")]
    public string? Gender { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Date of Birth")]
    public string? Dob { get; set; }

    [StringLength(200, ErrorMessage = "Street cannot exceed 200 characters")]
    [Display(Name = "Street")]
    public string? Street { get; set; }

    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    [Display(Name = "City")]
    public string? City { get; set; }

    [StringLength(100, ErrorMessage = "State cannot exceed 100 characters")]
    [Display(Name = "State")]
    public string? State { get; set; }
}

/// <summary>
/// ViewModel for user login.
/// </summary>
public class UserLoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// ViewModel for editing a user profile.
/// </summary>
public class UserEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "Gender cannot exceed 20 characters")]
    [Display(Name = "Gender")]
    public string? Gender { get; set; }

    [Display(Name = "Date of Birth")]
    public string? Dob { get; set; }

    [StringLength(200, ErrorMessage = "Street cannot exceed 200 characters")]
    [Display(Name = "Street")]
    public string? Street { get; set; }

    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    [Display(Name = "City")]
    public string? City { get; set; }

    [StringLength(100, ErrorMessage = "State cannot exceed 100 characters")]
    [Display(Name = "State")]
    public string? State { get; set; }
}
