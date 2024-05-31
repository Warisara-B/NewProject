using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment.Scholarship
{
    [Table("ScholarshipReserveBudgets")]
    public class ScholarshipReserveBudget
    {
        public Guid ScholarshipId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        [ForeignKey(nameof(ScholarshipId))]
		public virtual Scholarship Scholarship { get; set; }
    }
}