using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

/// <summary>
/// ViewModel for tour list/index page.
/// </summary>
public class TourListViewModel
{
    public int TourId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Locations { get; set; } = string.Empty;
    public string? Pic { get; set; }
}

/// <summary>
/// ViewModel for tour details page.
/// </summary>
public class TourDetailsViewModel
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
/// ViewModel for creating a new tour.
/// </summary>
public class TourCreateViewModel
{
    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(20, ErrorMessage = "Tour name must not exceed 20 characters.")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required.")]
    [StringLength(20, ErrorMessage = "Place must not exceed 20 characters.")]
    [Display(Name = "Place")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Number of days is required.")]
    [Range(1, 99, ErrorMessage = "Days must be between 1 and 99.")]
    [Display(Name = "Days")]
    public int Days { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Locations are required.")]
    [StringLength(100, ErrorMessage = "Locations must not exceed 100 characters.")]
    [Display(Name = "Locations")]
    public string Locations { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour information is required.")]
    [StringLength(200, ErrorMessage = "Tour information must not exceed 200 characters.")]
    [Display(Name = "Tour Information")]
    public string TourInfo { get; set; } = string.Empty;

    [Display(Name = "Tour Picture")]
    public IFormFile? PicFile { get; set; }
}

/// <summary>
/// ViewModel for editing an existing tour.
/// </summary>
public class TourEditViewModel
{
    public int TourId { get; set; }

    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(20, ErrorMessage = "Tour name must not exceed 20 characters.")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required.")]
    [StringLength(20, ErrorMessage = "Place must not exceed 20 characters.")]
    [Display(Name = "Place")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Number of days is required.")]
    [Range(1, 99, ErrorMessage = "Days must be between 1 and 99.")]
    [Display(Name = "Days")]
    public int Days { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be greater than 0.")]
    [DataType(DataType.Currency)]
    [Display(Name = "Price")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Locations are required.")]
    [StringLength(100, ErrorMessage = "Locations must not exceed 100 characters.")]
    [Display(Name = "Locations")]
    public string Locations { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour information is required.")]
    [StringLength(200, ErrorMessage = "Tour information must not exceed 200 characters.")]
    [Display(Name = "Tour Information")]
    public string TourInfo { get; set; } = string.Empty;

    public string? Pic { get; set; }

    [Display(Name = "New Tour Picture")]
    public IFormFile? PicFile { get; set; }
}

/// <summary>
/// ViewModel for tour delete confirmation page.
/// </summary>
public class TourDeleteViewModel
{
    public int TourId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
