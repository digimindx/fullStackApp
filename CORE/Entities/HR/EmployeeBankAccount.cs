using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CORE.Entities.HR
{
    [Table("EmployeeBankAccounts", Schema = "HR")]
    public class EmployeeBankAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BankAccountID { get; set; }

        [Required]
        public int EmployeeID { get; set; }

        public int? BankCode { get; set; }

        public int? BranchCode { get; set; }

        public int? CityCode { get; set; }

        [Required]
        [MaxLength(30)]
        public string AccountNumber { get; set; } = string.Empty;

        [MaxLength(34)]
        public string? IBAN { get; set; }

        [MaxLength(100)]
        public string? BankName { get; set; }

        [MaxLength(100)]
        public string? BankAddress1 { get; set; }

        [MaxLength(100)]
        public string? BankAddress2 { get; set; }

        public int? CountryCode { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        [Column(TypeName = "date")]
        public DateOnly ValidFrom { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        [Column(TypeName = "date")]
        public DateOnly? ValidTo { get; set; }

        // Navigation property
        [ForeignKey(nameof(EmployeeID))]
        public virtual Employee? Employee { get; set; }
    }
}