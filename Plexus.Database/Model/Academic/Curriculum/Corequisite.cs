using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("Corequisites")]
	public class Corequisite
	{
		public Guid CurriculumVersionId { get; set; }

		public Guid CourseId { get; set; }

		public Guid CorequisiteCourseId { get; set; }

		[ForeignKey(nameof(CurriculumVersionId))]
		public virtual CurriculumVersion CurriculumVersion { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }

		[ForeignKey(nameof(CorequisiteCourseId))]
		public virtual Course CorequisiteCourse { get; set; }
	}
}

