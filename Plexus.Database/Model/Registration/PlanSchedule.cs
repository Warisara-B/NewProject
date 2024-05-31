using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Registration
{
	[Table("PlanSchedules")]
	public class PlanSchedule
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid PlanId { get; set; }

		public DateTime CreatedAt { get; set; }

		[ForeignKey(nameof(PlanId))]
		public virtual Plan Plan { get; set; }

		public virtual IEnumerable<PlanScheduleSection> PlanSections { get; set; }
	}
}

