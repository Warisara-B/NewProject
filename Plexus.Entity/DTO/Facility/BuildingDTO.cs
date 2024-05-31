using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Facility
{
    public class CreateBuildingDTO
	{
		public string Name { get; set; }

		public string Code { get; set; }

		public Guid CampusId { get; set; }

		public bool IsActive { get; set; }

		public IEnumerable<BuildingLocalizationDTO>? Localizations { get; set; }
	}

	public class BuildingDTO : CreateBuildingDTO
    {
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
    }

	public class BuildingAvailableTimeDTO
    {
		public DayOfWeek Day { get; set; }

		public TimeSpan? FromTime { get; set; }

        public TimeSpan? ToTime { get; set; }
    }

	public class BuildingLocalizationDTO
    {
		public LanguageCode Language { get; set; }

		public string? Name { get; set; }
    }
}

