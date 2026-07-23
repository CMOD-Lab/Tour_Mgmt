using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

/// <summary>ViewModel for booking display.</summary>
public class BookingViewModel
{
    public int BookingId { get; set; }
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public DateTime BookingDate { get; set; }
}

/// <summary>ViewModel for booking create form.</summary>
public class BookingFormViewModel
{
    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(50, ErrorMessage = "Tour name must not exceed 50 characters.")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required.")]
    [StringLength(50, ErrorMessage = "Place must not exceed 50 characters.")]
    [Display(Name = "Place")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(50, ErrorMessage = "Email must not exceed 50 characters.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name must not exceed 50 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
}
