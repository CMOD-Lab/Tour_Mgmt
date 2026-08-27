namespace TourManagement.Domain.Entities;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class UserInfo
{
    /// <summary>Gets or sets the email address (primary key).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Gets or sets the gender.</summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>Gets or sets the hashed password.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Gets or sets the date of birth.</summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>Gets or sets the street address.</summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>Gets or sets the city.</summary>
    public string City { get; set; } = string.Empty;

    /// <summary>Gets or sets the state.</summary>
    public string State { get; set; } = string.Empty;

    /// <summary>Gets or sets whether the user account is active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Gets or sets the date the user registered.</summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>Navigation property for bookings made by this user.</summary>
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
