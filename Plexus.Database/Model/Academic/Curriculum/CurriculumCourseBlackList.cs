using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("CurriculumCourseBlackLists")]
	public class CurriculumCourseBlackList
	{
		public Guid CurriculumVersionId { get; set; }

		public Guid CourseId { get; set; }

        [ForeignKey(nameof(CurriculumVersionId))]
		public virtual CurriculumVersion CurriculumVersion { get; set; }

        [ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }
	}
}

