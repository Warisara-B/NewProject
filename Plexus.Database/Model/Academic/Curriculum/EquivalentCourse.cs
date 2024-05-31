using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("EquivalentCourses")]
	public class EquivalentCourse
	{	
		public Guid CurriculumVersionId { get; set; }

		public Guid CourseId { get; set; }

		public Guid EquivalenceCourseId { get; set; }

		[ForeignKey(nameof(CurriculumVersionId))]
		public virtual CurriculumVersion CurriculumVersion { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }

		[ForeignKey(nameof(EquivalenceCourseId))]
		public virtual Course Equivalence { get; set; }
	}
}