namespace Plexus.Entity.DTO.Facility
{
    public class UpdateRoomFacilityDTO
    {
        public Guid FacilityId { get; set; }

        public int Amount { get; set; }

        public bool IsActive { get; set; }
    }

    public class RoomFacilityDTO : UpdateRoomFacilityDTO
    {
        public DateTime? CreatedAt { get; set; }
    }
}