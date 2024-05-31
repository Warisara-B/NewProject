using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Facility
{
    public class CreateCampusDTO
	{
		public string Name { get; set; }

		public string Code { get; set; }

		public string? Address1 { get; set; }

		public string? Address2 { get; set; }

		public string? ContactNumber { get; set; }

		public bool IsActive { get; set; }

		public IEnumerable<CampusLocalizationDTO> Localizations { get; set; }
	}

	public class CampusDTO : CreateCampusDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}

	public class CampusLocalizationDTO
    {
		public LanguageCode Language { get; set; }

        public string Name { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }
    }
}

