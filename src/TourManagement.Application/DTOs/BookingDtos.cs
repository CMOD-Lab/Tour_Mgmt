namespace TourManagement.Application.DTOs;

/// <summary>
/// Data transfer object for Booking.
/// </summary>
public class BookingDto
{
    public int BookingId { get; set; }
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public int? TourId { get; set; }
}

/// <summary>
/// DTO for creating a new booking.
/// </summary>
public class BookingCreateDto
{
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public int? TourId { get; set; }
}

/// <summary>
/// DTO for updating an existing booking.
/// </summary>
public class BookingUpdateDto
{
    public int BookingId { get; set; }
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public int? TourId { get; set; }
}
