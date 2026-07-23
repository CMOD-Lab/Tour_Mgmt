namespace Tour_Management.Domain.DTOs;

/// <summary>Data transfer object for Booking read operations.</summary>
public class BookingDto
{
    public int BookingId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public int? TourId { get; set; }
    public int? UserId { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>Data transfer object for Booking create operations.</summary>
public class BookingCreateDto
{
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public int? TourId { get; set; }
    public int? UserId { get; set; }
    public string CreatedBy { get; set; } = "system";
}

/// <summary>Data transfer object for Booking update operations.</summary>
public class BookingUpdateDto
{
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? ModifiedBy { get; set; }
}
