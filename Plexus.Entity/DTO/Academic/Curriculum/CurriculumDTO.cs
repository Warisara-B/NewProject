using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Academic.Curriculum
{
	public class CreateCurriculumDTO
    {
		public Guid AcademicLevelId { get; set; }

		public Guid FacultyId { get; set; }

		public Guid? DepartmentId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
		
		public string? FormalName { get; set; }
		
		public string? Abbreviation { get; set; }

		public string? Description { get; set; }

		public bool IsActive { get; set; }

		public IEnumerable<CurriculumLocalizationDTO>? Localizations { get; set; }
    }

	public class CurriculumDTO : CreateCurriculumDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public IEnumerable<CurriculumVersionDTO> Versions { get; set; } = Enumerable.Empty<CurriculumVersionDTO>();
	}

	public class CurriculumLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? FormalName { get; set; }

        public string? Abbreviation { get; set; }
    }
}

