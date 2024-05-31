using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Research;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Research
{
    [Table("ResearchProfiles")]
    public class ResearchProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ResearchTemplateId { get; set; }

        public string? Name { get; set; }

        public Term StartTerm { get; set; }

        public Term? CompletedTerm { get; set; }

        public ResearchStatus? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        [ForeignKey(nameof(ResearchTemplateId))]
        public virtual ResearchTemplate ResearchTemplate { get; set; }

        public virtual IEnumerable<ResearchMember> ResearchMembers { get; set; }

        public virtual IEnumerable<ResearchResource>? Resources { get; set; }
    }
}