namespace TourBooking.Domain.Entities;

/// <summary>
/// Represents a tour booking made by a user.
/// </summary>
public class Booking
{
    /// <summary>Gets or sets the booking identifier.</summary>
    public int TourId { get; set; }

    /// <summary>Gets or sets the tour name.</summary>
    public string? TourName { get; set; }

    /// <summary>Gets or sets the place/destination.</summary>
    public string? Place { get; set; }

    /// <summary>Gets or sets the email of the user who made the booking.</summary>
    public string? Email { get; set; }

    /// <summary>Gets or sets the first name of the user who made the booking.</summary>
    public string? FirstName { get; set; }

    /// <summary>Gets or sets the user associated with this booking.</summary>
    public UserInfo? User { get; set; }
}
