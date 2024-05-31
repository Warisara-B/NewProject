using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic.Curriculum;

namespace Plexus.Database.Model.Localization.Academic.Curriculum
{
    [Table("CurriculumVersions", Schema = "localization")]
	public class CurriculumVersionLocalization
	{
        public Guid CurriculumVersionId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? DegreeName { get; set; }

		public string? Description { get; set; }

        public string? Abbreviation { get; set; }

        [ForeignKey(nameof(CurriculumVersionId))]
        public virtual CurriculumVersion CurriculumVersion { get; set; }
    }
}

