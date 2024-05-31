using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Facility;

namespace Plexus.Database.Model.Localization.Facility
{
    [Table("Rooms", Schema = "localization")]
	public class RoomLocalization
	{
		public Guid RoomId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(RoomId))]
		public virtual Room Room { get; set; }
	}
}

