using Plexus.Database.Enum.Academic;

namespace Plexus.Entity.DTO.Academic
{
	public class CreateTermDTO
	{

		public int Year { get; set; }

		public string Number { get; set; }

		public TermType Type { get; set; }

		public CollegeCalendarType CollegeCalendarType { get; set; }

		public Guid AcademicLevelId { get; set; }

		public DateTime StartedAt { get; set; }

		public DateTime EndedAt { get; set; }

		public bool IsCurrent { get; set; }

		public bool IsRegistration { get; set; }

		public bool IsAdvising { get; set; }

		public bool IsSurveyed { get; set; }

		public int TotalWeeks { get; set; }
	}

	public class TermDTO : CreateTermDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}

