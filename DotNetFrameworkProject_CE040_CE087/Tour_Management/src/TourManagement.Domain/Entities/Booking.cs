namespace TourManagement.Domain.Entities;

/// <summary>
/// Represents a tour booking made by a user.
/// </summary>
public class Booking
{
    /// <summary>Gets or sets the unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the tour name.</summary>
    public string TourName { get; set; } = string.Empty;

    /// <summary>Gets or sets the place/destination.</summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>Gets or sets the email of the person who booked.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the first name of the person who booked.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the foreign key for the tour.</summary>
    public int? TourId { get; set; }

    /// <summary>Gets or sets the foreign key for the user.</summary>
    public int? UserId { get; set; }

    /// <summary>Gets or sets the booking date.</summary>
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets the creation date.</summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets the last modified date.</summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>Gets or sets whether the booking is active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Navigation property for the tour.</summary>
    public Tour? Tour { get; set; }

    /// <summary>Navigation property for the user.</summary>
    public User? User { get; set; }
}
