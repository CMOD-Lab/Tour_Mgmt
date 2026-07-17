using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour_Management.Models
{
    /// <summary>
    /// Entity model for booking table.
    /// Used by Entity Framework Core DbContext for cloud-native data access.
    /// </summary>
    [Table("booking")]
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TOUR_ID { get; set; }

        [MaxLength(200)]
        public string TOUR_NAME { get; set; }

        [MaxLength(200)]
        public string PLACE { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(200)]
        public string FirstName { get; set; }
    }
}
