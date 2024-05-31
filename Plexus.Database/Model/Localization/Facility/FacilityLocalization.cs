using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using FacilityModel = Plexus.Database.Model.Facility.Facility;

namespace Plexus.Database.Model.Localization.Facility
{
    [Table("Facilities", Schema = "localization")]
	public class FacilityLocalization
	{
		public Guid FacilityId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(FacilityId))]
		public virtual FacilityModel Facility { get; set; }
	}
}