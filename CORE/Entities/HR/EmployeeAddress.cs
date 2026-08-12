using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CORE.Entities.HR
{
    [Table("EmployeeAddresses", Schema = "HR")]
    public class EmployeeAddress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        [Required]
        [MaxLength(1)]
        [RegularExpression("^[CP]$", ErrorMessage = "AddressType must be 'C' (Current) or 'P' (Permanent)")]
        public char AddressType { get; set; } = 'C';

        public int? CityCode { get; set; }

        public int? CountryCode { get; set; }

        [Required]
        [MaxLength(200)]
        public string AddressText { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? NearestPoliceStation { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        [MaxLength(20)]
        public string? MobileNumber { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required]
        public bool IsPrimary { get; set; } = true;

        [Required]
        [Column(TypeName = "date")]
        public DateOnly ValidFrom { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [Column(TypeName = "date")]
        public DateOnly? ValidTo { get; set; }

        // Audit fields matching enhanced schema
        [Required]
        [MaxLength(128)]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [MaxLength(128)]
        public string? LastModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? LastModifiedDate { get; set; }

    }
}