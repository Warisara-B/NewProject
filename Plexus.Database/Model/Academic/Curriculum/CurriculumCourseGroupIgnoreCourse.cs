using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("CurriculumCourseGroupIgnoreCourses")]
	public class CurriculumCourseGroupIgnoreCourse
	{
		public Guid CourseGroupId { get; set; }

		public Guid CourseId { get; set; }

        [ForeignKey(nameof(CourseGroupId))]
		public virtual CurriculumCourseGroup CurriculumCourseGroup { get; set; }

        [ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }
	}
}

