using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Facility;

namespace Plexus.Database.Model.Localization.Facility
{
    [Table("Buildings", Schema = "localization")]
	public class BuildingLocalization
	{
		public Guid BuildingId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(BuildingId))]
		public virtual Building Building { get; set; }
	}
}

