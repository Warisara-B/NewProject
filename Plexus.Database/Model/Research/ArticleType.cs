using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Research;

namespace Plexus.Database.Model.Research
{
    [Table("ArticleTypes")]
    public class ArticleType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public virtual IEnumerable<ArticleTypeLocalization> Localizations { get; set; }
    }
}