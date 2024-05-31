using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Research;

namespace Plexus.Database.Model.Localization.Research
{
    [Table("ResearchCategory", Schema = "localization")]
    public class ResearchCategoryLocalization
    {
        public Guid ResearchCategoryId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        [ForeignKey(nameof(ResearchCategoryId))]
        public virtual ResearchCategory ResearchCategory { get; set; }

    }
}