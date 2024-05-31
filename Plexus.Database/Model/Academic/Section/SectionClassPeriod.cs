using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Facility;

namespace Plexus.Database.Model.Academic.Section
{
    [Table("SectionClassPeriods")]
    public class SectionClassPeriod
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid SectionId { get; set; }

        public Guid? RoomId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public DayOfWeek Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual Room? Room { get; set; }

        public virtual IEnumerable<SectionClassPeriodInstructor> Instructors { get; set; }
    }
}