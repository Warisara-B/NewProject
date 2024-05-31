using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Research;

namespace Plexus.Database.Model.Research
{
    [Table("ResearchCommittees")]
    public class ResearchCommittee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ResearchProcessId { get; set; }

        public Guid InstructorId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public CommitteePosition Position { get; set; }

        [ForeignKey(nameof(ResearchProcessId))]
        public virtual ResearchProcess Process { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public virtual Employee Instructor { get; set; }
    }
}