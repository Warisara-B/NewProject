using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic;

namespace Plexus.Database.Model.Academic
{
    [Table("AcademicCalendars")]
    public class AcademicCalendar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public AcademicCalendarEventType Type { get; set; }

        public DateTime? StartedAt { get; set; }

        public DateTime? EndedAt { get; set; }

        public string? Location { get; set; }
    }
}