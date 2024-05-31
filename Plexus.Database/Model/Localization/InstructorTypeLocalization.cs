using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model.Localization
{
    [Table("InstructorTypes", Schema = "localization")]
    public class InstructorTypeLocalization
    {
        public Guid InstructorTypeId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

		public string? Name { get; set; }

        [ForeignKey(nameof(InstructorTypeId))]
		public virtual InstructorType InstructorType { get; set; }
    }
}