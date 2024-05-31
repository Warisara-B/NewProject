using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model
{
    [Table("AudienceGroups")]
    public class AudienceGroup
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Conditions { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}