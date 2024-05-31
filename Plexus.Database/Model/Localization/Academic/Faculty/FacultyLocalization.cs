using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model.Localization.Academic.Faculty
{
    [Table("Faculties", Schema = "localization")]
	public class FacultyLocalization
	{
		public Guid FacultyId { get; set; }

		public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		public string? FormalName { get; set; }

        [ForeignKey(nameof(FacultyId))]
		public virtual Model.Academic.Faculty.Faculty Faculty { get; set; }
	}
}

