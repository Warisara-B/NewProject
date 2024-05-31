using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Facility;

namespace Plexus.Database.Model.Facility
{
    [Table("Campuses")]
	public class Campus
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public string? Address1 { get; set; }

		public string? Address2 { get; set; }

		public string? ContactNumber { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		public bool IsActive { get; set; }

		public virtual IEnumerable<CampusLocalization> Localizations { get; set; }
	}
}
