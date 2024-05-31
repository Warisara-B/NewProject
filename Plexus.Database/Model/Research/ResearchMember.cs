using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Research
{
    [Table("ResearchMembers")]
    public class ResearchMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ResearchId { get; set; }

        public Guid StudentId { get; set; }

        [ForeignKey(nameof(ResearchId))]
        public ResearchProfile Research { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }
    }
}