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
    public DateTime? DateOfBirth { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public bool IsAdmin { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}

/// <summary>
/// ViewModel for user registration.
/// </summary>
public class UserRegisterViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(256, ErrorMessage = "Email must not exceed 256 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Gender")]
    public string? Gender { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(200)]
    [Display(Name = "Street")]
    public string? Street { get; set; }

    [StringLength(100)]
    [Display(Name = "City")]
    public string? City { get; set; }

    [StringLength(100)]
    [Display(Name = "State")]
    public string? State { get; set; }
}

/// <summary>
/// ViewModel for user login.
/// </summary>
public class UserLoginViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
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

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Gender")]
    public string? Gender { get; set; }

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(200)]
    [Display(Name = "Street")]
    public string? Street { get; set; }

    [StringLength(100)]
    [Display(Name = "City")]
    public string? City { get; set; }

    [StringLength(100)]
    [Display(Name = "State")]
    public string? State { get; set; }

    public bool IsActive { get; set; } = true;
}
