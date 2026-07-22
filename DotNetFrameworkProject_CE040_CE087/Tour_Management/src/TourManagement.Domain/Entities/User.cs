namespace TourManagement.Domain.Entities;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User
{
    /// <summary>Gets or sets the unique identifier.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the user's email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's gender.</summary>
    public string? Gender { get; set; }

    /// <summary>Gets or sets the hashed password.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Gets or sets the date of birth.</summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>Gets or sets the street address.</summary>
    public string? Street { get; set; }

    /// <summary>Gets or sets the city.</summary>
    public string? City { get; set; }

    /// <summary>Gets or sets the state.</summary>
    public string? State { get; set; }

    /// <summary>Gets or sets whether the user is an admin.</summary>
    public bool IsAdmin { get; set; } = false;

    /// <summary>Gets or sets the creation date.</summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets the last modified date.</summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>Gets or sets whether the user is active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Navigation property for bookings.</summary>
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
