namespace TourManagement.Domain.Entities;

/// <summary>
/// Represents a tour booking made by a user.
/// </summary>
public class Booking
{
    /// <summary>Gets or sets the booking identifier.</summary>
    public int BookingId { get; set; }

    /// <summary>Gets or sets the tour name at time of booking.</summary>
    public string TourName { get; set; } = string.Empty;

    /// <summary>Gets or sets the place/destination at time of booking.</summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>Gets or sets the email of the user who booked.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the first name of the user who booked.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the date the booking was created.</summary>
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets the foreign key to the Tour.</summary>
    public int? TourId { get; set; }

    /// <summary>Navigation property: the tour that was booked.</summary>
    public Tour? Tour { get; set; }

    /// <summary>Navigation property: the user who made the booking.</summary>
    public UserInfo? User { get; set; }
}
