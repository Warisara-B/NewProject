using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Research;

namespace Plexus.Database.Model.Localization.Research
{
    [Table("ResearchProcesses", Schema = "localization")]
    public class ResearchProcessLocalization
    {
        public Guid ProcessId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        [ForeignKey(nameof(ProcessId))]
        public virtual ResearchProcess? ResearchProcess { get; set; }
    }
}