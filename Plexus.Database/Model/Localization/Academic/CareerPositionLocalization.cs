using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Localization.Academic
{
    [Table("CareerPositions", Schema = "localization")]
	public class CareerPositionLocalization
	{
		public Guid CareerPositionId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Abbreviation { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(CareerPositionId))]
		public virtual CareerPosition CareerPosition { get; set; }
    }
}

