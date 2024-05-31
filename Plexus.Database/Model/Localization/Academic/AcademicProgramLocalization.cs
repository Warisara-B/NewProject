using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Localization.Academic
{
    [Table("AcademicPrograms", Schema = "localization")]
	public class AcademicProgramLocalization
	{
		public Guid AcademicProgramId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }
		public string? FormalName { get; set; }

        [ForeignKey(nameof(AcademicProgramId))]
		public virtual AcademicProgram AcademicProgram { get; set; }
	}
}

