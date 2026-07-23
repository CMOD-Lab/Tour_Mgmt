using System.ComponentModel.DataAnnotations;
using Tour_Management.Domain.DTOs;

namespace Tour_Management.Web.ViewModels;

/// <summary>ViewModel for displaying a user.</summary>
public class UserViewModel
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string Role { get; set; } = "User";
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    /// <summary>Manually maps from UserDto to UserViewModel.</summary>
    public static UserViewModel FromDto(UserDto dto) => new()
    {
        UserId = dto.UserId,
        Email = dto.Email,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Gender = dto.Gender,
        DateOfBirth = dto.DateOfBirth,
        Street = dto.Street,
        City = dto.City,
        State = dto.State,
        Role = dto.Role,
        CreatedDate = dto.CreatedDate,
        IsActive = dto.IsActive
    };
}

/// <summary>ViewModel for user registration.</summary>
public class RegisterViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(256, ErrorMessage = "Email must not exceed 256 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Gender { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(200)]
    public string? Street { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    /// <summary>Manually maps to UserCreateDto.</summary>
    public UserCreateDto ToCreateDto() => new()
    {
        Email = Email,
        FirstName = FirstName,
        LastName = LastName,
        Gender = Gender,
        Password = Password,
        DateOfBirth = DateOfBirth,
        Street = Street,
        City = City,
        State = State
    };
}

/// <summary>ViewModel for user login.</summary>
public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }

    /// <summary>Manually maps to UserLoginDto.</summary>
    public UserLoginDto ToLoginDto() => new()
    {
        Email = Email,
        Password = Password
    };
}

/// <summary>ViewModel for editing a user profile.</summary>
public class UserEditViewModel
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(20)]
    public string? Gender { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(200)]
    public string? Street { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? State { get; set; }

    /// <summary>Manually maps from UserDto to UserEditViewModel.</summary>
    public static UserEditViewModel FromDto(UserDto dto) => new()
    {
        UserId = dto.UserId,
        FirstName = dto.FirstName,
        LastName = dto.LastName,
        Gender = dto.Gender,
        DateOfBirth = dto.DateOfBirth,
        Street = dto.Street,
        City = dto.City,
        State = dto.State
    };

    /// <summary>Manually maps to UserUpdateDto.</summary>
    public UserUpdateDto ToUpdateDto() => new()
    {
        FirstName = FirstName,
        LastName = LastName,
        Gender = Gender,
        DateOfBirth = DateOfBirth,
        Street = Street,
        City = City,
        State = State
    };
}
