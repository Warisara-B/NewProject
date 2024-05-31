namespace Plexus.Entity.DTO.Facility
{
    public class CreateRoomTypeDTO
	{

		public string Name { get; set; }
	}

	public class RoomTypeDTO : CreateRoomTypeDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}