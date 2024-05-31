using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic.Curriculum;

namespace Plexus.Database.Model.Localization.Academic.Curriculum
{
    [Table("AcademicSpecializations", Schema = "localization")]
	public class AcademicSpecializationLocalization
	{
        public Guid AcademicSpecializationId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Abbreviation { get; set; }

        [ForeignKey(nameof(AcademicSpecializationId))]
        public virtual AcademicSpecialization AcademicSpecialization { get; set; }
    }
}