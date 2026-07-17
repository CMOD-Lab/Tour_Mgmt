using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour_Management.Models
{
    /// <summary>
    /// Represents a UserInfo entity mapped to the UserInfo table in Azure SQL Database.
    /// </summary>
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(100)]
        [Column("LastName")]
        public string? LastName { get; set; }

        [MaxLength(20)]
        [Column("Gender")]
        public string? Gender { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("Password")]
        public string Password { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("dob")]
        public string? Dob { get; set; }

        [MaxLength(300)]
        [Column("Street")]
        public string? Street { get; set; }

        [MaxLength(100)]
        [Column("City")]
        public string? City { get; set; }

        [MaxLength(100)]
        [Column("State")]
        public string? State { get; set; }
    }
}
