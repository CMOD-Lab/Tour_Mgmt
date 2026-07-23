namespace Tour_Management.Domain.Entities;

/// <summary>
/// Represents a user registered in the system.
/// </summary>
public class UserInfo
{
    /// <summary>Gets or sets the unique identifier for the user.</summary>
    public int UserId { get; set; }

    /// <summary>Gets or sets the user's email address (used as login).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's first name.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's last name.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's gender.</summary>
    public string? Gender { get; set; }

    /// <summary>Gets or sets the hashed password.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's date of birth.</summary>
    public DateTime? Dob { get; set; }

    /// <summary>Gets or sets the user's street address.</summary>
    public string? Street { get; set; }

    /// <summary>Gets or sets the user's city.</summary>
    public string? City { get; set; }

    /// <summary>Gets or sets the user's state.</summary>
    public string? State { get; set; }

    /// <summary>Gets or sets the date the user registered.</summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>Gets or sets whether the user account is active.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Navigation property for bookings made by this user.</summary>
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
