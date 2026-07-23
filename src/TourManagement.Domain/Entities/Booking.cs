namespace TourManagement.Domain.Entities;

/// <summary>
/// Represents a tour booking in the Tour Management system.
/// </summary>
public class Booking
{
    /// <summary>Gets or sets the booking identifier.</summary>
    public int BookingId { get; set; }

    /// <summary>Gets or sets the tour name for the booking.</summary>
    public string? TourName { get; set; }

    /// <summary>Gets or sets the place/destination for the booking.</summary>
    public string? Place { get; set; }

    /// <summary>Gets or sets the email of the user who made the booking.</summary>
    public string? Email { get; set; }

    /// <summary>Gets or sets the first name of the user who made the booking.</summary>
    public string? FirstName { get; set; }

    /// <summary>Gets or sets the booking date.</summary>
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    /// <summary>Navigation property: the user who made this booking.</summary>
    public UserInfo? User { get; set; }
}
