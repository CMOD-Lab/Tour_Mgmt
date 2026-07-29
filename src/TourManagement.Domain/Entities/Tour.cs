namespace TourManagement.Domain.Entities;

/// <summary>
/// Represents a tour package offered in the system.
/// </summary>
public class Tour
{
    /// <summary>Gets or sets the tour identifier.</summary>
    public int TourId { get; set; }

    /// <summary>Gets or sets the tour name.</summary>
    public string TourName { get; set; } = string.Empty;

    /// <summary>Gets or sets the main destination place.</summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>Gets or sets the number of days for the tour.</summary>
    public int Days { get; set; }

    /// <summary>Gets or sets the price of the tour.</summary>
    public decimal Price { get; set; }

    /// <summary>Gets or sets the locations visited during the tour.</summary>
    public string Locations { get; set; } = string.Empty;

    /// <summary>Gets or sets detailed information about the tour.</summary>
    public string TourInfo { get; set; } = string.Empty;

    /// <summary>Gets or sets the picture filename for the tour.</summary>
    public string? Pic { get; set; }

    /// <summary>Gets or sets the date the tour was created.</summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets the date the tour was last modified.</summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>Gets or sets whether the tour is active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Navigation property: bookings for this tour.</summary>
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
