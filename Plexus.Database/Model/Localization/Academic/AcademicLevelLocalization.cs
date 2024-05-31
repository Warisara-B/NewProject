using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Localization.Academic
{
    [Table("AcademicLevels", Schema = "localization")]
	public class AcademicLevelLocalization
	{
		public Guid AcademicLevelId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		public string? FormalName { get; set; }

        [ForeignKey(nameof(AcademicLevelId))]
		public virtual AcademicLevel AcademicLevel { get; set; }
	}
}

