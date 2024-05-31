using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Facility.Reservation;

namespace Plexus.Database.Model.Facility.Reservation
{
    [Table("RoomReserveSlots")]
    public class RoomReserveSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public ReservationStatus Status { get; set; }

        public string? Remark { get; set; }

        public Guid? RoomReserveRequestId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; }

        [ForeignKey(nameof(RoomReserveRequestId))]
        public RoomReserveRequest? RoomReserveRequest { get; set; }
    }
}