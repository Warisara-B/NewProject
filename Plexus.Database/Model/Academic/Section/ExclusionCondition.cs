using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Section
{
    [Table("ExclusionConditions")]
    public class ExclusionCondition
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid SectionId { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public string Conditions { get; set; }

        public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

        [ForeignKey(nameof(SectionId))]
		public virtual Section Section { get; set; }
    }
}