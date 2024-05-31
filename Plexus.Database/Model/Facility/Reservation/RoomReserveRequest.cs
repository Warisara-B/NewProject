using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Facility.Reservation;

namespace Plexus.Database.Model.Facility.Reservation
{
    [Table("RoomReserveRequests")]
    public class RoomReserveRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Title { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public SenderType SenderType { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public TimeSpan StartedAt { get; set; }

		public TimeSpan EndedAt { get; set; }

		public string? RepeatedOn { get; set; }

        public Guid RoomId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public UsageType UsageType { get; set; }

        public string Description { get; set; }

        public string? Remark { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public ReservationStatus Status { get; set; }

        public string RequesterName { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; }

        public virtual IEnumerable<RoomReserveSlot> Slots { get; set; }
    }
}