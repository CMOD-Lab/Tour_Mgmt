namespace Tour_Management.Application.DTOs;

/// <summary>DTO for displaying tour information.</summary>
public class TourDto
{
    public int TourId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Locations { get; set; } = string.Empty;
    public string TourInfo { get; set; } = string.Empty;
    public string? PicturePath { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>DTO for creating a new tour.</summary>
public class TourCreateDto
{
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Locations { get; set; } = string.Empty;
    public string TourInfo { get; set; } = string.Empty;
    public string? PicturePath { get; set; }
}

/// <summary>DTO for updating an existing tour.</summary>
public class TourUpdateDto
{
    public int TourId { get; set; }
    public string TourName { get; set; } = string.Empty;
    public string Place { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal Price { get; set; }
    public string Locations { get; set; } = string.Empty;
    public string TourInfo { get; set; } = string.Empty;
    public string? PicturePath { get; set; }
}
