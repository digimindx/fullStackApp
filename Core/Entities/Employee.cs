using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Employees", Schema = "HR")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database will generate ID (IDENTITY)
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? MiddleName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; } // Kept standard C# casing 'Email', EF maps this easily or we handle it in DbContext

        [MaxLength(50)]
        public string? Username { get; set; }

        public string? Password { get; set; }
    }
}