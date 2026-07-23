using System.ComponentModel.DataAnnotations;
using Tour_Management.Domain.DTOs;

namespace Tour_Management.Web.ViewModels;

/// <summary>ViewModel for displaying a booking.</summary>
public class BookingViewModel
{
    public int BookingId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public int? TourId { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }

    /// <summary>Manually maps from BookingDto to BookingViewModel.</summary>
    public static BookingViewModel FromDto(BookingDto dto) => new()
    {
        BookingId = dto.BookingId,
        TourName = dto.TourName,
        Place = dto.Place,
        Email = dto.Email,
        FirstName = dto.FirstName,
        TourId = dto.TourId,
        BookingDate = dto.BookingDate,
        CreatedDate = dto.CreatedDate,
        IsActive = dto.IsActive
    };
}

/// <summary>ViewModel for creating a new booking.</summary>
public class BookingCreateViewModel
{
    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(200, ErrorMessage = "Tour name must not exceed 200 characters.")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required.")]
    [StringLength(200, ErrorMessage = "Place must not exceed 200 characters.")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(256, ErrorMessage = "Email must not exceed 256 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    public int? TourId { get; set; }

    /// <summary>Manually maps to BookingCreateDto.</summary>
    public BookingCreateDto ToCreateDto() => new()
    {
        TourName = TourName,
        Place = Place,
        Email = Email,
        FirstName = FirstName,
        TourId = TourId,
        CreatedBy = Email
    };
}

/// <summary>ViewModel for editing a booking.</summary>
public class BookingEditViewModel
{
    public int BookingId { get; set; }

    [Required(ErrorMessage = "Tour name is required.")]
    [StringLength(200, ErrorMessage = "Tour name must not exceed 200 characters.")]
    [Display(Name = "Tour Name")]
    public string TourName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Place is required.")]
    [StringLength(200, ErrorMessage = "Place must not exceed 200 characters.")]
    public string Place { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(256, ErrorMessage = "Email must not exceed 256 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name must not exceed 100 characters.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Manually maps from BookingDto to BookingEditViewModel.</summary>
    public static BookingEditViewModel FromDto(BookingDto dto) => new()
    {
        BookingId = dto.BookingId,
        TourName = dto.TourName,
        Place = dto.Place,
        Email = dto.Email,
        FirstName = dto.FirstName
    };

    /// <summary>Manually maps to BookingUpdateDto.</summary>
    public BookingUpdateDto ToUpdateDto() => new()
    {
        TourName = TourName,
        Place = Place,
        Email = Email,
        FirstName = FirstName
    };
}
