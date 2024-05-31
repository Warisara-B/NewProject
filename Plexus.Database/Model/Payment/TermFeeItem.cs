using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Payment;

namespace Plexus.Database.Model.Payment
{
    [Table("TermFeeItems")]
    public class TermFeeItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid TermFeePackageId { get; set; }

        public Guid FeeItemId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public RecurringType RecurringType { get; set; }

        public decimal Amount { get; set; }

        public string? TermNo { get; set; }

        public int? TermRunning { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public TermType? TermType { get; set; }

        public string? Conditions { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(TermFeePackageId))]
        public virtual TermFeePackage TermFeePackage { get; set; }

        [ForeignKey(nameof(FeeItemId))]
        public virtual FeeItem FeeItem { get; set; }
    }
}