using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Announcement
{
	public class BookmarkNews
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid NewsId { get; set; }

		public Guid? StudentId { get; set; }

		public Guid? InstructorId { get; set; }

		public DateTime BookmarkAt { get; set; }

		[ForeignKey(nameof(NewsId))]
		public virtual News News { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student? Student { get; set; }

		[ForeignKey(nameof(InstructorId))]
		public virtual Employee? Instructor { get; set; }
	}
}

