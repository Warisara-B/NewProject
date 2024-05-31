using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Research
{
    [Table("ResearchResources")]
    public class ResearchResource
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? ResearchProfileId { get; set; }

        public Guid? ResearchProcessId { get; set; }

        public string? FileName { get; set; }

        public string? Url { get; set; }

        [ForeignKey(nameof(ResearchProfileId))]
        public virtual ResearchProfile? Research { get; set; }

        [ForeignKey(nameof(ResearchProcessId))]
        public virtual ResearchProcess? Process { get; set; }
    }
}