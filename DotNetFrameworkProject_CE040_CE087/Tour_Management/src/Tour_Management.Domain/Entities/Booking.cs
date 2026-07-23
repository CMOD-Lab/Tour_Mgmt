namespace Tour_Management.Domain.Entities;

/// <summary>
/// Represents a tour booking made by a user.
/// </summary>
public class Booking
{
    /// <summary>Gets or sets the unique identifier for the booking.</summary>
    public int BookingId { get; set; }

    /// <summary>Gets or sets the name of the tour booked.</summary>
    public string TourName { get; set; } = string.Empty;

    /// <summary>Gets or sets the place/destination of the tour.</summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>Gets or sets the email of the person who booked.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the first name of the person who booked.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the date the booking was made.</summary>
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets whether the booking is active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Gets or sets the foreign key for the tour.</summary>
    public int? TourId { get; set; }

    /// <summary>Navigation property for the associated tour.</summary>
    public Tour? Tour { get; set; }
}
