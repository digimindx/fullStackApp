using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CORE.Entities.HR
{
    [Table("EmployeeAccounts", Schema = "HR")]
    public class EmployeeAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [MaxLength(1)]
        public char? Gender { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string PasswordSalt { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "datetime")]
        public DateTime? LastLoginDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public bool IsLocked { get; set; } = false;

        [Column(TypeName = "datetime")]
        public DateTime? LockedUntil { get; set; }

        public int FailedLoginAttempts { get; set; }

        [MaxLength(500)]
        public string? RefreshToken { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Audit fields
        [Required]
        [MaxLength(128)]
        public string CreatedBy { get; set; } = string.Empty;

        [Column(TypeName = "datetime")]
        public DateTime? LastModifiedDate { get; set; }

        [MaxLength(128)]
        public string? LastModifiedBy { get; set; }
    }
}