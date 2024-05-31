using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Registration
{
	[Table("Slots")]
	public class Slot
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid PeriodId { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public DateTime StartedAt { get; set; }

		public DateTime EndedAt { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		public bool IsActive { get; set; }

		public bool IsSpecialSlot { get; set; }

		[ForeignKey(nameof(PeriodId))]
		public virtual Period Period { get; set; }

		public virtual IEnumerable<SlotCondition> SlotConditions { get; set; }
	}
}

