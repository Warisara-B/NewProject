using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Registration
{
	[Table("Plans")]
	public class Plan
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid StudentId { get; set; }

		public Guid TermId { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public PlanType Type { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student Student { get; set; }

		[ForeignKey(nameof(TermId))]
		public virtual Term Term { get; set; }

		public virtual IEnumerable<PlanCourse> PlanCourses { get; set; }

		public virtual IEnumerable<PlanSchedule> PlanSchedules { get; set; }
	}
}

