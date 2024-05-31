using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Payment;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Payment
{
    public class Invoice
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public string Number { get; set; }

		public decimal Amount { get; set; }

		public Guid StudentId { get; set; }

		public Guid TermId { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public InvoiceStatus Status { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student Student { get; set; }

		[ForeignKey(nameof(TermId))]
		public virtual Term Term { get; set; }
	}
}

