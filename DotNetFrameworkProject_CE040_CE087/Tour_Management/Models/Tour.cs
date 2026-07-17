using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour_Management.Models
{
    /// <summary>
    /// Represents a Tour entity mapped to the Tour table in Azure SQL Database.
    /// </summary>
    [Table("Tour")]
    public class Tour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("TOUR_NAME")]
        public string TourName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [Column("PLACE")]
        public string Place { get; set; } = string.Empty;

        [Column("DAYS")]
        public int Days { get; set; }

        [Column("PRICE")]
        public decimal Price { get; set; }

        [MaxLength(500)]
        [Column("LOCATIONS")]
        public string? Locations { get; set; }

        [MaxLength(250)]
        [Column("TOUR_INFO")]
        public string? TourInfo { get; set; }

        [MaxLength(500)]
        [Column("pic")]
        public string? Pic { get; set; }
    }
}
