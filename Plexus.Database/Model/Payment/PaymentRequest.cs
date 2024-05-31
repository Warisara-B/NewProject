using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment
{
    public class PaymentRequest
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public long InvoiceId { get; set; }

		public string Number { get; set; }

		public decimal Amount { get; set; }

		public string Reference1 { get; set; }

		public string Reference2 { get; set; }

		public DateTime ExpiryDateTime { get; set; }

		public bool IsPaid { get; set; }

		public DateTime? PaymentSuccessDateTime { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		[ForeignKey(nameof(InvoiceId))]
		public virtual Invoice Invoice { get; set; }
	}
}

