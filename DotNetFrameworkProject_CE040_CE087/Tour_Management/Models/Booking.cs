using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour_Management.Models
{
    /// <summary>
    /// Represents a Booking entity mapped to the booking table in Azure SQL Database.
    /// </summary>
    [Table("booking")]
    public class Booking
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

        [Required]
        [MaxLength(200)]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [Column("FirstName")]
        public string FirstName { get; set; } = string.Empty;
    }
}
