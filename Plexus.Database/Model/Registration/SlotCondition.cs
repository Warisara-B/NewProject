using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Registration
{
	[Table("SlotConditions")]
	public class SlotCondition
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid SlotId { get; set; }

		public string Conditions { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(SlotId))]
		public virtual Slot Slot { get; set; }
	}
}

