using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment.Scholarship
{
    [Table("ScholarshipFeeItemTransactions")]
    public class ScholarshipFeeItemTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public Guid ScholarshipId { get; set; }

        public Guid FeeItemId { get; set; }

        public decimal? Percentage { get; set; }

        public decimal? Amount { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(ScholarshipId))]
		public virtual Scholarship Scholarship { get; set; }

        [ForeignKey(nameof(FeeItemId))]
		public virtual FeeItem FeeItem { get; set; }
    }
}

