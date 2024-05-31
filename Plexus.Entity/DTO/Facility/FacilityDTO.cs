using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Facility
{
    public class CreateFacilityDTO
	{
		public string Name { get; set; }

		public IEnumerable<FacilityLocalizationDTO>? Localizations { get; set; }
    }

	public class FacilityDTO : CreateFacilityDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}

	public class FacilityLocalizationDTO
    {
		public LanguageCode Language { get; set; }

		public string? Name { get; set; }
    }
}