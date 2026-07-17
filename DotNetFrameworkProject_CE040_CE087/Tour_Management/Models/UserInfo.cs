using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tour_Management.Models
{
    /// <summary>
    /// Entity model for UserInfo table.
    /// Used by Entity Framework Core DbContext for cloud-native data access.
    /// </summary>
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(200)]
        public string FirstName { get; set; }

        [MaxLength(200)]
        public string LastName { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [MaxLength(200)]
        public string Password { get; set; }

        [MaxLength(50)]
        public string dob { get; set; }

        [MaxLength(300)]
        public string Street { get; set; }

        [MaxLength(200)]
        public string City { get; set; }

        [MaxLength(200)]
        public string State { get; set; }
    }
}
