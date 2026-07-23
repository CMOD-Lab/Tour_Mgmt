namespace TourManagement.Domain.DTOs;

/// <summary>Booking data transfer object for read operations.</summary>
public class BookingDto
{
    public int BookingId { get; set; }
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public DateTime BookingDate { get; set; }
}

/// <summary>Booking data transfer object for create operations.</summary>
public class BookingCreateDto
{
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
}

/// <summary>Booking data transfer object for update operations.</summary>
public class BookingUpdateDto
{
    public string? TourName { get; set; }
    public string? Place { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
}
