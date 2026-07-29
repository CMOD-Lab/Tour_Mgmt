namespace TourManagement.Domain.Entities;

/// <summary>
/// Represents a booking made by a user for a tour.
/// </summary>
public class Booking
{
    /// <summary>Gets or sets the booking identifier.</summary>
    public int BookingId { get; set; }

    /// <summary>Gets or sets the tour name at time of booking.</summary>
    public string TourName { get; set; } = string.Empty;

    /// <summary>Gets or sets the place/destination at time of booking.</summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>Gets or sets the email of the user who made the booking.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the first name of the user who made the booking.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the tour identifier (foreign key).</summary>
    public int? TourId { get; set; }

    /// <summary>Gets or sets the date the booking was created.</summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets whether the booking is active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Navigation property: the tour associated with this booking.</summary>
    public Tour? Tour { get; set; }

    /// <summary>Navigation property: the user who made this booking.</summary>
    public UserInfo? User { get; set; }
}
