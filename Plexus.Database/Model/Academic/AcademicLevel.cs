using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Academic;

namespace Plexus.Database.Model.Academic
{
	[Table("AcademicLevels")]
	public class AcademicLevel
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string FormalName { get; set; }

		public bool IsActive { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		public virtual IEnumerable<AcademicLevelLocalization> Localizations { get; set; }
	}
}
