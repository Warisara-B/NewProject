using Plexus.Database.Enum.Facility.Reservation;

namespace Plexus.Entity.DTO.Facility.Reservation
{
    public class UpdateRoomReserveSlotDTO
    {
        public Guid Id { get; set; }

        public string? Remark { get; set; }
    }

    public class RoomReserveSlotDTO : UpdateRoomReserveSlotDTO
    {
        public Guid RoomId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public ReservationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}