using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Registration
{
	[Table("CoursePrerequisites")]
	public class CoursePrerequisite
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid CourseId { get; set; }

		public IEnumerable<string> Conditions { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime? DeactivatedAt { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }

		public virtual IEnumerable<CurriculumCoursePrerequisite> Curriculums { get; set; }
	}
}

