namespace TourManagement.Application.DTOs;

/// <summary>
/// Data transfer object for UserInfo read operations.
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public string? Dob { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string Role { get; set; } = "User";
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Data transfer object for creating a new User (registration).
/// </summary>
public class UserCreateDto
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public string Password { get; set; } = string.Empty;
    public string? Dob { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
}

/// <summary>
/// Data transfer object for updating an existing User.
/// </summary>
public class UserUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public string? Dob { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string ModifiedBy { get; set; } = "system";
}
