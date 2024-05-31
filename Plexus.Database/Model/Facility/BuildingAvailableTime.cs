using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Facility
{
    [Table("BuildingAvailableTimes")]
    public class BuildingAvailableTime
	{
		public Guid BuildingId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public DayOfWeek Day { get; set; }

		public TimeSpan? FromTime { get; set; }

		public TimeSpan? ToTime { get; set; }

        [ForeignKey(nameof(BuildingId))]
		public virtual Building Building { get; set; }
	}
}

