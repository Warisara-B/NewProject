using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Registration
{
	[Table("PlanCourses")]
	public class PlanCourse
	{
		public Guid PlanId { get; set; }

		public Guid CourseId { get; set; }

		[ForeignKey(nameof(PlanId))]
		public virtual Plan Plan { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }
	}
}

