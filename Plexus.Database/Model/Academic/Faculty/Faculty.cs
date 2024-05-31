using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Academic.Faculty;

namespace Plexus.Database.Model.Academic.Faculty
{
	[Table("Faculties")]
	public class Faculty
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Code { get; set; }

		public string Name { get; set; }

		public string? FormalName { get; set; }

		public string? LogoImagePath { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		public bool IsActive { get; set; }

		public virtual IEnumerable<FacultyLocalization> Localizations { get; set; }
	}
}

