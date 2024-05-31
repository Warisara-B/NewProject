using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Academic.Section;

namespace Plexus.Database.Model.Registration
{
	[Table("PlanScheduleSections")]
	public class PlanScheduleSection
	{
		public Guid PlanScheduleId { get; set; }

		public Guid SectionId { get; set; }

		[ForeignKey(nameof(PlanScheduleId))]
		public virtual PlanSchedule PlanSchedule { get; set; }

		[ForeignKey(nameof(SectionId))]
		public virtual Section Section { get; set; }
	}
}

