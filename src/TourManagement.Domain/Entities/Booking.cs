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

    /// <summary>Gets or sets the place/city for the booking.</summary>
    public string? Place { get; set; }

    /// <summary>Gets or sets the email of the user who made the booking.</summary>
    public string? Email { get; set; }

    /// <summary>Gets or sets the first name of the person who made the booking.</summary>
    public string? FirstName { get; set; }

    /// <summary>Gets or sets the tour identifier (foreign key).</summary>
    public int? TourId { get; set; }

    /// <summary>Navigation property: the tour associated with this booking.</summary>
    public Tour? Tour { get; set; }

    /// <summary>Navigation property: the user who made this booking.</summary>
    public UserInfo? User { get; set; }
}
