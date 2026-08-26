using System.ComponentModel.DataAnnotations;

namespace TourBooking.Web.ViewModels;

/// <summary>
/// ViewModel for displaying a tour.
/// </summary>
public class TourViewModel
{
    public int TourId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Locations { get; set; } = string.Empty;
    public string TourInfo { get; set; } = string.Empty;
    public string? Pic { get; set; }
}

/// <summary>
/// ViewModel for creating or editing a tour.
/// </summary>
public class TourCreateEditViewModel
{
    public int TourId { get; set; }

    [Required(ErrorMessage = "Tour name is required")]
    [StringLength(20)]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required")]
    [StringLength(20)]
    [Display(Name = "Place")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Number of days is required")]
    [Range(1, 99)]
    [Display(Name = "Days")]
    public int Days { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999999)]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Locations are required")]
    [StringLength(100)]
    [Display(Name = "Locations")]
    public string Locations { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour information is required")]
    [StringLength(200)]
    [Display(Name = "Tour Information")]
    public string TourInfo { get; set; } = string.Empty;

    [Display(Name = "Tour Picture")]
    public IFormFile? PicFile { get; set; }

    public string? ExistingPic { get; set; }
}
