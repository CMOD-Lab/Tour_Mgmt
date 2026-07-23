using System.ComponentModel.DataAnnotations;
using Tour_Management.Domain.DTOs;

namespace Tour_Management.Web.ViewModels;

/// <summary>ViewModel for displaying a tour in the list.</summary>
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
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }

    /// <summary>Manually maps from TourDto to TourViewModel.</summary>
    public static TourViewModel FromDto(TourDto dto) => new()
    {
        TourId = dto.TourId,
        TourName = dto.TourName,
        Place = dto.Place,
        Days = dto.Days,
        Price = dto.Price,
        Locations = dto.Locations,
        TourInfo = dto.TourInfo,
        Pic = dto.Pic,
        CreatedDate = dto.CreatedDate,
        IsActive = dto.IsActive
    };
}

/// <summary>ViewModel for creating a new tour.</summary>
public class TourCreateViewModel
{
    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(200, ErrorMessage = "Tour name must not exceed 200 characters.")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required.")]
    [StringLength(200, ErrorMessage = "Place must not exceed 200 characters.")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Days is required.")]
    [Range(1, 365, ErrorMessage = "Days must be between 1 and 365.")]
    public int Days { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Locations are required.")]
    [StringLength(500, ErrorMessage = "Locations must not exceed 500 characters.")]
    public string Locations { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour information is required.")]
    [StringLength(250, ErrorMessage = "Tour information must not exceed 250 characters.")]
    [Display(Name = "Tour Information")]
    public string TourInfo { get; set; } = string.Empty;

    public IFormFile? PicFile { get; set; }

    /// <summary>Manually maps to TourCreateDto.</summary>
    public TourCreateDto ToCreateDto(string? picFileName = null) => new()
    {
        TourName = TourName,
        Place = Place,
        Days = Days,
        Price = Price,
        Locations = Locations,
        TourInfo = TourInfo,
        Pic = picFileName,
        CreatedBy = "admin"
    };
}

/// <summary>ViewModel for editing an existing tour.</summary>
public class TourEditViewModel
{
    public int TourId { get; set; }

    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(200, ErrorMessage = "Tour name must not exceed 200 characters.")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required.")]
    [StringLength(200, ErrorMessage = "Place must not exceed 200 characters.")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Days is required.")]
    [Range(1, 365, ErrorMessage = "Days must be between 1 and 365.")]
    public int Days { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Locations are required.")]
    [StringLength(500, ErrorMessage = "Locations must not exceed 500 characters.")]
    public string Locations { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tour information is required.")]
    [StringLength(250, ErrorMessage = "Tour information must not exceed 250 characters.")]
    [Display(Name = "Tour Information")]
    public string TourInfo { get; set; } = string.Empty;

    public string? ExistingPic { get; set; }
    public IFormFile? PicFile { get; set; }

    /// <summary>Manually maps from TourDto to TourEditViewModel.</summary>
    public static TourEditViewModel FromDto(TourDto dto) => new()
    {
        TourId = dto.TourId,
        TourName = dto.TourName,
        Place = dto.Place,
        Days = dto.Days,
        Price = dto.Price,
        Locations = dto.Locations,
        TourInfo = dto.TourInfo,
        ExistingPic = dto.Pic
    };

    /// <summary>Manually maps to TourUpdateDto.</summary>
    public TourUpdateDto ToUpdateDto(string? picFileName = null) => new()
    {
        TourName = TourName,
        Place = Place,
        Days = Days,
        Price = Price,
        Locations = Locations,
        TourInfo = TourInfo,
        Pic = picFileName ?? ExistingPic,
        ModifiedBy = "admin"
    };
}
