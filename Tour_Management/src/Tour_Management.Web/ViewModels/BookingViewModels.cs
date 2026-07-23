using System.ComponentModel.DataAnnotations;

namespace Tour_Management.Web.ViewModels;

/// <summary>ViewModel for displaying a booking.</summary>
public class BookingViewModel
{
    public int BookingId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

/// <summary>ViewModel for creating a booking.</summary>
public class BookingCreateViewModel
{
    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(50, ErrorMessage = "Tour name cannot exceed 50 characters.")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required.")]
    [StringLength(50, ErrorMessage = "Place cannot exceed 50 characters.")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
}

/// <summary>ViewModel for editing a booking.</summary>
public class BookingEditViewModel : BookingCreateViewModel
{
    public int BookingId { get; set; }
}
