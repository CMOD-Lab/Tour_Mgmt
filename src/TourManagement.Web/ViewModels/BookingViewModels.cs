using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

/// <summary>
/// ViewModel for booking list/index page.
/// </summary>
public class BookingListViewModel
{
    public int BookingId { get; set; }
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
}

/// <summary>
/// ViewModel for booking details page.
/// </summary>
public class BookingDetailsViewModel
{
    public int BookingId { get; set; }
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public int? TourId { get; set; }
}

/// <summary>
/// ViewModel for creating a new booking (Order page).
/// </summary>
public class BookingCreateViewModel
{
    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(50, ErrorMessage = "Tour name must not exceed 50 characters.")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required.")]
    [StringLength(50, ErrorMessage = "Place must not exceed 50 characters.")]
    [Display(Name = "Place / City")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(50, ErrorMessage = "Email must not exceed 50 characters.")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name must not exceed 50 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    public int? TourId { get; set; }
}

/// <summary>
/// ViewModel for booking delete confirmation page.
/// </summary>
public class BookingDeleteViewModel
{
    public int BookingId { get; set; }
    public string? TourName { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
}
