using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic.Advising;

namespace Plexus.Database.Model.Academic.Advising
{
    [Table("AdvisingSlots")]
    public class AdvisingSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? StudentId { get; set; }

        public Guid? TermId { get; set; }

        public Guid InstructorId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public AdvisingSlotStatus Status { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime EndedAt { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public virtual Employee Instructor { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student? Student { get; set; }

        [ForeignKey(nameof(TermId))]
        public virtual Term? Term { get; set; }
    }
}