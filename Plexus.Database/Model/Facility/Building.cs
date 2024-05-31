using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Facility;

namespace Plexus.Database.Model.Facility
{
    [Table("Buildings")]
    public class Building
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public Guid CampusId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(CampusId))]
        public virtual Campus? Campus { get; set; }

        public virtual IEnumerable<BuildingAvailableTime> AvailableTimes { get; set; }

        public virtual IEnumerable<BuildingLocalization> Localizations { get; set; }
    }
}

