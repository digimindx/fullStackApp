using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CORE.Entities.Financial;

[Table("FinancialAccounts")]
public class FinancialAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AccountID { get; set; }

    [Required]
    public int EmployeeID { get; set; }

    [Required]
    [MaxLength(50)]
    public string AccountNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? AccountName { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Balance { get; set; }

    [MaxLength(10)]
    public string? Currency { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "datetime")]
    public DateTime? LastModifiedDate { get; set; }
}
