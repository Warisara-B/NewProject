using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
	[Table("CurriculumCourses")]
	public class CurriculumCourse
	{
		public Guid CourseGroupId { get; set; }

		public Guid CourseId { get; set; }

		public Guid? RequiredGradeId { get; set; }

		[ForeignKey(nameof(CourseGroupId))]
		public virtual CurriculumCourseGroup CourseGroup { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }

		[ForeignKey(nameof(RequiredGradeId))]
		public virtual Grade? RequiredGrade { get; set; }
	}
}

