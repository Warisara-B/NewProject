using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Localization.Research;

namespace Plexus.Database.Model.Research
{
    [Table("ResearchTemplates")]
    public class ResearchTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }

        public virtual IEnumerable<ResearchTemplateSequence> Sequences { get; set; }

        public virtual IEnumerable<ResearchTemplateLocalization> Localizations { get; set; }
    }
}