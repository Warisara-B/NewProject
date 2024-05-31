using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Advising
{
	public class CourseRecommendation
	{
		public Guid StudentId { get; set; }

		public Guid TermId { get; set; }

		public Guid CourseId { get; set; }

		public bool IsRequired { get; set; }

		public Guid? InstructorId { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student Student { get; set; }

		[ForeignKey(nameof(TermId))]
		public virtual Term Term { get; set; }

		[ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }

		[ForeignKey(nameof(InstructorId))]
		public virtual Employee? Instructor { get; set; }
	}
}

