using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Model.Facility;

namespace Plexus.Database.Model.Academic.Section
{
	[Table("SectionDetails")]
	public class SectionDetail
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public Guid SectionId { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public DayOfWeek Day { get; set; }

		public TimeSpan StartTime { get; set; }

		public TimeSpan EndTime { get; set; }

		public Guid? RoomId { get; set; }

		public Guid? InstructorId { get; set; }
		public Guid? TeachingTypeId { get; set; }

		public string? Remark { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		[ForeignKey(nameof(SectionId))]
		public virtual Section Section { get; set; }

		[ForeignKey(nameof(RoomId))]
		public virtual Room? Room { get; set; }

		[ForeignKey(nameof(InstructorId))]
		public virtual Employee? Instructor { get; set; }

		[ForeignKey(nameof(TeachingTypeId))]
		public virtual TeachingType? TeachingType { get; set; }
	}
}

