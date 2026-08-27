namespace TourManagement.Domain.Entities;

/// <summary>
/// Represents a tour booking made by a user.
/// </summary>
public class Booking
{
    /// <summary>Gets or sets the unique booking identifier.</summary>
    public int BookingId { get; set; }

    /// <summary>Gets or sets the name of the tour booked.</summary>
    public string TourName { get; set; } = string.Empty;

    /// <summary>Gets or sets the place/city of the user.</summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>Gets or sets the email of the user who booked.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the first name of the user who booked.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the tour ID (foreign key).</summary>
    public int? TourId { get; set; }

    /// <summary>Gets or sets the booking date.</summary>
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets whether the booking is active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Navigation property for the associated tour.</summary>
    public Tour? Tour { get; set; }
}
