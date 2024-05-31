using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment.Scholarship
{
    [Table("ScholarshipFeeItems")]
    public class ScholarshipFeeItem
    {
        public Guid ScholarshipId { get; set; }

        public Guid FeeItemId { get; set; }

        public decimal? Percentage { get; set; }

        public decimal? Amount { get; set; }

        [ForeignKey(nameof(ScholarshipId))]
		public virtual Scholarship Scholarship { get; set; }

        [ForeignKey(nameof(FeeItemId))]
		public virtual FeeItem FeeItem { get; set; }
    }
}