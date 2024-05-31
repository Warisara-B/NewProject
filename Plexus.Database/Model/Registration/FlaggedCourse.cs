using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Section;

namespace Plexus.Database.Model.Registration
{
	[Table("FlaggedCourses")]
	public class FlaggedCourse
	{
		public Guid StudentId { get; set; }

		public Guid CourseId { get; set; }

		public Guid TermId { get; set; }

		public Guid? SectionId { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public FlaggedCourseSource Source { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public FlaggedCourseType Type { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student Student { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }

		[ForeignKey(nameof(TermId))]
		public virtual Term Term { get; set; }

		[ForeignKey(nameof(SectionId))]
		public virtual Section Section { get; set; }
	}
}

