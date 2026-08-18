using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CORE.Entities.HR
{
    [Table("Employees", Schema = "HR")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeID { get; set; }

        public string? EmployeeNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? MotherName { get; set; }

        [MaxLength(1)]
        public char? Gender { get; set; }

        [Column(TypeName = "date")]
        public DateOnly DateOfBirth { get; set; }

        [MaxLength(100)]
        public string? BirthPlace { get; set; }

        [Required]
        public bool IsBornAbroad { get; set; } = false;

        public int? NationalityCode { get; set; }

        [Required]
        public bool HasDoubleNationality { get; set; } = false;

        [MaxLength(1)]
        public char? MaritalStatus { get; set; }

        [MaxLength(1)]
        public char? SocialCategory { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        [Required]
        [MaxLength(128)]
        public string CreatedBy { get; set; } = string.Empty;

        [Column(TypeName = "datetime")]
        public DateTime? LastModifiedDate { get; set; }

        [MaxLength(128)]
        public string? LastModifiedBy { get; set; }

        [Required]
        public bool IsActive { get; set; } = false;

        // Navigation property
        public EmployeeAccount? EmployeeAccount { get; set; }
    }
}