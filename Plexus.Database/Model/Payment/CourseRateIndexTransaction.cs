using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Payment;

namespace Plexus.Database.Model.Payment
{
    [Table("CourseRateIndexTransactions")]
    public class CourseRateIndexTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public Guid CourseRateId { get; set; }

        public Guid RateTypeId { get; set; }

        public int Index { get; set; }

        public decimal Amount { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public CalculationType? CalculationType { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(CourseRateId))]
        public virtual CourseRate CourseRate { get; set; }

        [ForeignKey(nameof(RateTypeId))]
        public virtual RateType RateType { get; set; }
    }
}

