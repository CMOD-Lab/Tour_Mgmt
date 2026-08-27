using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

/// <summary>
/// ViewModel for displaying a booking in the list.
/// </summary>
public class BookingListViewModel
{
    public int BookingId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
}

/// <summary>
/// ViewModel for displaying booking details.
/// </summary>
public class BookingDetailsViewModel
{
    public int BookingId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public int? TourId { get; set; }
    public DateTime BookingDate { get; set; }
}

/// <summary>
/// ViewModel for creating a new booking (Order page).
/// </summary>
public class BookingCreateViewModel
{
    [Required(ErrorMessage = "Your name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    [Display(Name = "Your Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Your city is required")]
    [StringLength(100, ErrorMessage = "City cannot exceed 100 characters")]
    [Display(Name = "Your City")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour name is required")]
    [StringLength(100, ErrorMessage = "Tour name cannot exceed 100 characters")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    public int? TourId { get; set; }
}

/// <summary>
/// ViewModel for deleting a booking (confirmation page).
/// </summary>
public class BookingDeleteViewModel
{
    public int BookingId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime BookingDate { get; set; }
}
