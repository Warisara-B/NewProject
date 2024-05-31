using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Facility
{
    [Table("RoomFacilities")]
    public class RoomFacility
    {
        public Guid RoomId { get; set; }

        public Guid FacilityId { get; set; }

        public int Amount { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [ForeignKey(nameof(RoomId))]
		public virtual Room Room { get; set; }

        [ForeignKey(nameof(FacilityId))]
		public virtual Facility Facility { get; set; }
    }
}