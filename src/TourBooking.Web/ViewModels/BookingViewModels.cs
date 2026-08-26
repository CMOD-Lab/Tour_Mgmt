using System.ComponentModel.DataAnnotations;

namespace TourBooking.Web.ViewModels;

/// <summary>
/// ViewModel for displaying a booking.
/// </summary>
public class BookingViewModel
{
    public int TourId { get; set; }
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
}

/// <summary>
/// ViewModel for creating a booking.
/// </summary>
public class BookingCreateViewModel
{
    [Required(ErrorMessage = "Tour name is required")]
    [StringLength(50)]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required")]
    [StringLength(50)]
    [Display(Name = "Place")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [StringLength(50)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [StringLength(50)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;
}
