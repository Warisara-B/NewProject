using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Facility
{
	public class CreateRoomDTO
	{
		public Guid? BuildingId { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public int? Floor { get; set; }

		public int? Capacity { get; set; }

		public Guid? RoomTypeId { get; set; }

		public int? ExaminationCapacity { get; set; }

		public bool IsActive { get; set; }

		public bool IsReservable { get; set; }

		public IEnumerable<RoomLocalizationDTO>? Localizations { get; set; }

		public IEnumerable<UpdateRoomFacilityDTO>? Facilities { get; set; }
	}

	public class RoomDTO : CreateRoomDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public new IEnumerable<RoomFacilityDTO>? Facilities { get; set; }
	}

	public class RoomLocalizationDTO
	{
		public LanguageCode Language { get; set; }

		public string? Name { get; set; }
	}
}