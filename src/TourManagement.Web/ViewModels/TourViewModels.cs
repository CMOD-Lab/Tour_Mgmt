using System.ComponentModel.DataAnnotations;

namespace TourManagement.Web.ViewModels;

/// <summary>
/// ViewModel for displaying a tour in the list.
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
/// ViewModel for displaying tour details.
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
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// ViewModel for creating a new tour.
/// </summary>
public class TourCreateViewModel
{
    [Required(ErrorMessage = "Tour name is required")]
    [StringLength(100, ErrorMessage = "Tour name cannot exceed 100 characters")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required")]
    [StringLength(100, ErrorMessage = "Place cannot exceed 100 characters")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Number of days is required")]
    [Range(1, 365, ErrorMessage = "Days must be between 1 and 365")]
    public int Days { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be a positive value")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Locations are required")]
    [StringLength(200, ErrorMessage = "Locations cannot exceed 200 characters")]
    public string Locations { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour information is required")]
    [StringLength(500, ErrorMessage = "Tour info cannot exceed 500 characters")]
    [Display(Name = "Tour Information")]
    public string TourInfo { get; set; } = string.Empty;

    [Display(Name = "Tour Image")]
    public IFormFile? PicFile { get; set; }
}

/// <summary>
/// ViewModel for editing an existing tour.
/// </summary>
public class TourEditViewModel
{
    public int TourId { get; set; }

    [Required(ErrorMessage = "Tour name is required")]
    [StringLength(100, ErrorMessage = "Tour name cannot exceed 100 characters")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required")]
    [StringLength(100, ErrorMessage = "Place cannot exceed 100 characters")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Number of days is required")]
    [Range(1, 365, ErrorMessage = "Days must be between 1 and 365")]
    public int Days { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be a positive value")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Locations are required")]
    [StringLength(200, ErrorMessage = "Locations cannot exceed 200 characters")]
    public string Locations { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour information is required")]
    [StringLength(500, ErrorMessage = "Tour info cannot exceed 500 characters")]
    [Display(Name = "Tour Information")]
    public string TourInfo { get; set; } = string.Empty;

    public string? Pic { get; set; }

    [Display(Name = "New Tour Image")]
    public IFormFile? PicFile { get; set; }

    public bool IsActive { get; set; } = true;
}

/// <summary>
/// ViewModel for deleting a tour (confirmation page).
/// </summary>
public class TourDeleteViewModel
{
    public int TourId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal Price { get; set; }
}
