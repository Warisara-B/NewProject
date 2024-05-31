using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Payment;

namespace Plexus.Database.Model.Payment
{
	[Table("FeeItems")]
	public class FeeItem
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid FeeGroupId { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public string? CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string? UpdatedBy { get; set; }

		[ForeignKey(nameof(FeeGroupId))]
		public virtual FeeGroup FeeGroup { get; set; }

		public virtual IEnumerable<FeeItemLocalization> Localizations { get; set; }
	}
}