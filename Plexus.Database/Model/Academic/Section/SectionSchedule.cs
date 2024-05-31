using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Facility;

namespace Plexus.Database.Model.Academic.Section
{
    [Table("SectionSchedules")]
    public class SectionSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid SectionId { get; set; }

        public Guid? RoomId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public DayOfWeek Day { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual Room? Room { get; set; }
    }
}