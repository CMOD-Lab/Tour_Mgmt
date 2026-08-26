namespace TourBooking.Application.DTOs;

/// <summary>
/// Data transfer object for Booking.
/// </summary>
public class BookingDto
{
    public int TourId { get; set; }
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
}

/// <summary>
/// Data transfer object for creating a new booking.
/// </summary>
public class BookingCreateDto
{
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
}

/// <summary>
/// Data transfer object for updating a booking.
/// </summary>
public class BookingUpdateDto
{
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
}
