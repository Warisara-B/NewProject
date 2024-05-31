using Plexus.Database.Enum.Facility.Reservation;

namespace Plexus.Entity.DTO.Facility.Reservation
{
    public class CreateRoomReserveRequestDTO
    {
        public string Title { get; set; }

        public SenderType SenderType { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public TimeSpan StartedAt { get; set; }

		public TimeSpan EndedAt { get; set; }
        
		public IEnumerable<DayOfWeek>? RepeatedOn { get; set; }

        public Guid RoomId { get; set; }

        public UsageType UsageType { get; set; }

        public string Description { get; set; }

        public string? Remark { get; set; }

        public string RequesterName { get; set; }
    }

    public class RoomReserveRequestDTO : CreateRoomReserveRequestDTO
    {
        public Guid Id { get; set; }

        public ReservationStatus Status { get; set; }

        public IEnumerable<RoomReserveSlotDTO> Slots { get; set; }

        public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
    }
}