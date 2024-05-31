using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Research;
using Plexus.Database.Model.Localization.Research;

namespace Plexus.Database.Model.Research
{
    [Table("ResearchTemplateSequences")]
    public class ResearchTemplateSequence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ResearchTemplateId { get; set; }

        public int Ordering { get; set; }

        public string? Name { get; set; }

        public string? FilePrefix { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public ResearchSequenceType Type { get; set; }

        [ForeignKey(nameof(ResearchTemplateId))]
        public ResearchTemplate? ResearchTemplate { get; set; }

        public virtual IEnumerable<ResearchTemplateSequenceLocalization>? Localizations { get; set; }
    }
}