using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Localization.Academic
{
    [Table("AcademicPositions", Schema = "localization")]
	public class AcademicPositionLocalization
	{
		public Guid AcademicPositionId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Abbreviation { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(AcademicPositionId))]
		public virtual AcademicPosition AcademicPosition { get; set; }
    }
}

