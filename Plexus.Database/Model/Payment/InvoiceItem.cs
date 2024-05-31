using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment
{
    public class InvoiceItem
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public long InvoiceId { get; set; }

		public Guid FeeItemId { get; set; }

		public string Description { get; set; }

		public decimal Amount { get; set; }

		public decimal TotalAmount { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(InvoiceId))]
		public virtual Invoice Invoice { get; set; }
	}
}

