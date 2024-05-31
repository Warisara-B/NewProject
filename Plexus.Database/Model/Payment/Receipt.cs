using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment
{
	[Table("Receipts")]
    public class Receipt
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public long InvoiceId { get; set; }

		public decimal Amount { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		[ForeignKey(nameof(InvoiceId))]
		public virtual Invoice Invoice { get; set; }
	}
}

