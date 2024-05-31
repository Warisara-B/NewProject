using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Section
{
    [Table("SectionSeatUsages")]
    public class SectionSeatUsage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		public Guid SectionId { get; set; }

		public Guid? StudentId { get; set; }

		public Guid SeatId { get; set; }

		public int Amount { get; set; }

		public Guid? ReferenceSeatId { get; set; }

		public Guid? StudyCourseId { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		[ForeignKey(nameof(SectionId))]
		public virtual Section Section { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student? Student { get; set; }

		[ForeignKey(nameof(SeatId))]
		public virtual SectionSeat Seat { get; set; }

		[ForeignKey(nameof(ReferenceSeatId))]
		public virtual SectionSeat? ReferenceSeat { get; set; }

		[ForeignKey(nameof(StudyCourseId))]
		public virtual StudyCourse? StudyCourse { get; set; }
	}
}

