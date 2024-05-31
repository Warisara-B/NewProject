using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Academic
{
	public class CreateAcademicLevelDTO
	{
		public string Name { get; set; }

		public string FormalName { get; set; }

		public bool IsActive { get; set; }

		public IEnumerable<AcademicLevelLocalizationDTO> Localizations { get; set; }
	}

	public class AcademicLevelDTO : CreateAcademicLevelDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}

	public class AcademicLevelLocalizationDTO
	{
		public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		public string? FormalName { get; set; }
	}
}

