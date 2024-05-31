using Plexus.Database.Enum.Academic.Section;

namespace Plexus.Entity.DTO.Academic
{
	public class CreateSectionDTO
	{
		public Guid CourseId { get; set; }

		public Guid TermId { get; set; }

		public string Number { get; set; }

		public int SeatLimit { get; set; }

		public int PlanningSeat { get; set; }

		public int MinimumSeat { get; set; }

		public int AvailableSeat { get; set; }

		public Guid? MainInstructorId { get; set; }

		public SectionStatus Status { get; set; }

		public bool IsWithdrawable { get; set; }

		public bool IsGhostSection { get; set; }

		public bool IsOutboundSection { get; set; }

		public DateTime StartedDate { get; set; }

		public int TotalWeeks { get; set; }

		public Guid? ParentSectionId { get; set; }

		public bool IsClosed { get; set; }

		public string? Remark { get; set; }
	}

	public class SectionDTO : CreateSectionDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}

