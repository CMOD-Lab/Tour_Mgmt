using System.ComponentModel.DataAnnotations;

namespace Tour_Management.Web.ViewModels;

/// <summary>ViewModel for displaying a user.</summary>
public class UserViewModel
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
}

/// <summary>ViewModel for user registration.</summary>
public class UserRegisterViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required.")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your password.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Street is required.")]
    [StringLength(50)]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50)]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(50)]
    public string State { get; set; } = string.Empty;
}

/// <summary>ViewModel for user login.</summary>
public class UserLoginViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }
}

/// <summary>ViewModel for editing a user profile.</summary>
public class UserEditViewModel
{
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required.")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required.")]
    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Street is required.")]
    [StringLength(50)]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50)]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(50)]
    public string State { get; set; } = string.Empty;
}
