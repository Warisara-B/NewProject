using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Facility;

namespace Plexus.Database.Model.Facility
{
    [Table("Rooms")]
    public class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int? Floor { get; set; }

        public Guid? BuildingId { get; set; }

        public int? Capacity { get; set; }

        public int? ExaminationCapacity { get; set; }

        public Guid? RoomTypeId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public bool IsReservable { get; set; }

        [ForeignKey(nameof(BuildingId))]
        public Building? Building { get; set; }

        [ForeignKey(nameof(RoomTypeId))]
        public RoomType? RoomType { get; set; }

        public virtual IEnumerable<RoomLocalization> Localizations { get; set; }

        public virtual IEnumerable<RoomFacility> Facilities { get; set; }
    }
}

