using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Facility;

namespace Plexus.Database.Model.Localization.Facility
{
    [Table("Campuses", Schema = "localization")]
	public class CampusLocalization
	{
		public Guid CampusId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		public string? Address1 { get; set; }

		public string? Address2 { get; set; }

        [ForeignKey(nameof(CampusId))]
		public virtual Campus Campus { get; set; }
	}
}