using System;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model.Localization.Academic.Curriculum
{
    [Table("Curriculums", Schema = "localization")]
	public class CurriculumLocalization
	{
		public Guid CurriculumId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

		public string? FormalName { get; set; }

		public string? Abbreviation { get; set; }

        [ForeignKey(nameof(CurriculumId))]
		public virtual Model.Academic.Curriculum.Curriculum Curriculum { get; set; }
	}
}