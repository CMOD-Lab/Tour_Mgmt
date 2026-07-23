using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

/// <summary>
/// ViewModel for displaying a booking.
/// </summary>
public class BookingViewModel
{
    public int Id { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
    public int? TourId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// ViewModel for creating a new booking (order).
/// </summary>
public class BookingCreateViewModel
{
    [Required(ErrorMessage = "Tour name is required")]
    [StringLength(200, ErrorMessage = "Tour name cannot exceed 200 characters")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required")]
    [StringLength(200, ErrorMessage = "Place cannot exceed 200 characters")]
    [Display(Name = "Place")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    public int? TourId { get; set; }
}

/// <summary>
/// ViewModel for editing an existing booking.
/// </summary>
public class BookingEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tour name is required")]
    [StringLength(200, ErrorMessage = "Tour name cannot exceed 200 characters")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required")]
    [StringLength(200, ErrorMessage = "Place cannot exceed 200 characters")]
    [Display(Name = "Place")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(200, ErrorMessage = "Email cannot exceed 200 characters")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
}
