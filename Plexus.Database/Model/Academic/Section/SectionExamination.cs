using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Model.Facility;

namespace Plexus.Database.Model.Academic.Section
{
    [Table("SectionExaminations")]
    public class SectionExamination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid SectionId { get; set; }

        public Guid? RoomId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public ExamType? ExamType { get; set; }

        public DateTime? Date { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [ForeignKey(nameof(SectionId))]
        public virtual Section Section { get; set; }

        [ForeignKey(nameof(RoomId))]
        public virtual Room? Room { get; set; }
    }
}