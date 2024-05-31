using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;

namespace Plexus.Entity.DTO.Academic.Curriculum
{
	public class CreateCurriculumVersionDTO
	{
		public Guid? CurriculumId { get; set; }
		public Guid AcademicLevelId { get; set; }
		public Guid FacultyId { get; set; }
		public Guid DepartmentId { get; set; }
		public Guid AcademicProgramId { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public string? DegreeName { get; set; }

		public string? Description { get; set; }

		public string? Abbreviation { get; set; }

		public decimal TotalYear { get; set; }
		public decimal TotalCredit { get; set; }
		public decimal ExpectedGraduatingCredit { get; set; }
		public DateTime ApprovedAt { get; set; }
		public int StartBatchCode { get; set; }
		public int EndBatchCode { get; set; }

		public CollegeCalendarType CollegeCalendarType { get; set; }

		public string? Remark { get; set; }

		public bool IsActive { get; set; }

		public IEnumerable<CurriculumVersionLocalizationDTO> Localizations { get; set; }
	}

	public class CurriculumVersionDTO : CreateCurriculumVersionDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}

	public class CurriculumVersionLocalizationDTO
	{
		public LanguageCode Language { get; set; }

		public string? Name { get; set; }
		public string? DegreeName { get; set; }
		public string? Description { get; set; }
		public string? Abbreviation { get; set; }
	}
}

