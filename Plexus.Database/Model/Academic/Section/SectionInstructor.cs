using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Section
{
	[Table("SectionInstructors")]
	public class SectionInstructor
	{
		public Guid SectionId { get; set; }

		public Guid InstructorId { get; set; }

		[ForeignKey(nameof(SectionId))]
		public virtual Section Section { get; set; }

		[ForeignKey(nameof(InstructorId))]
		public virtual Employee Instructor { get; set; }
	}
}

