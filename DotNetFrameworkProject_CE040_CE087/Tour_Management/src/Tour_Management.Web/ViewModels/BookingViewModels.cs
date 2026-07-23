using System.ComponentModel.DataAnnotations;

namespace Tour_Management.Web.ViewModels;

/// <summary>
/// ViewModel for creating a new booking.
/// </summary>
public class BookingCreateViewModel
{
    [Required(ErrorMessage = "Tour name is required")]
    [StringLength(200)]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required")]
    [StringLength(200)]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    public int? TourId { get; set; }
}

/// <summary>
/// ViewModel for displaying a booking in a list.
/// </summary>
public class BookingIndexViewModel
{
    public int BookingId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
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
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// ViewModel for editing a booking.
/// </summary>
public class BookingEditViewModel
{
    public int BookingId { get; set; }

    [Required(ErrorMessage = "Tour name is required")]
    [StringLength(200)]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required")]
    [StringLength(200)]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}

/// <summary>
/// ViewModel for deleting a booking.
/// </summary>
public class BookingDeleteViewModel
{
    public int BookingId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
}
