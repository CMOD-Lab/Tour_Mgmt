using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour_Management.Models
{
    /// <summary>
    /// Entity model for Tour table.
    /// Used by Entity Framework Core DbContext for cloud-native data access.
    /// </summary>
    [Table("Tour")]
    public class Tour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TOUR_ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string TOUR_NAME { get; set; }

        [MaxLength(200)]
        public string PLACE { get; set; }

        public int? DAYS { get; set; }

        public decimal? PRICE { get; set; }

        [MaxLength(500)]
        public string LOCATIONS { get; set; }

        [MaxLength(1000)]
        public string TOUR_INFO { get; set; }

        [MaxLength(500)]
        public string pic { get; set; }
    }
}
