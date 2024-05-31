using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Research;

namespace Plexus.Database.Model.Localization.Research
{
    [Table("ResearchTemplates", Schema = "localization")]
    public class ResearchTemplateLocalization
    {
        public Guid ResearchTemplateId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        [ForeignKey(nameof(ResearchTemplateId))]
        public virtual ResearchTemplate? ResearchTemplate { get; set; }
    }
}