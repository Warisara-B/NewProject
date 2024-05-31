using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Research;

namespace Plexus.Database.Model.Localization.Research
{
    [Table("ArticleTypes", Schema = "localization")]
    public class ArticleTypeLocalization
    {
        public Guid ArticleTypeId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        [ForeignKey(nameof(ArticleTypeId))]
        public virtual ArticleType ArticleType { get; set; }
    }
}