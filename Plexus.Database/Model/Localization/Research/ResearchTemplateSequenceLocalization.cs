using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Research;

namespace Plexus.Database.Model.Localization.Research
{
    [Table("ResearchTemplateSequences", Schema = "localization")]
    public class ResearchTemplateSequenceLocalization
    {
        public Guid SequenceId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        [ForeignKey(nameof(SequenceId))]
        public virtual ResearchTemplateSequence? ResearchTemplateSequence { get; set; }
    }
}