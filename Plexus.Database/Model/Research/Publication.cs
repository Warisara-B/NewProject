using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Research;

namespace Plexus.Database.Model.Research
{
    [Table("Publications")]
    public class Publication
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ArticleTypeId { get; set; }

        public string Authors { get; set; }

        public int Pages { get; set; }

        public int Year { get; set; }

        public string? CitationPages { get; set; }

        public string? CitationDOI { get; set; }

        [ForeignKey(nameof(ArticleTypeId))]
        public virtual ArticleType ArticleType { get; set; }
    }
}