using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model.Localization
{
    public class InstructorRoleLocalization
    {
        public Guid InstructorRoleId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        [ForeignKey(nameof(InstructorRoleId))]
        public virtual InstructorRole InstructorRole { get; set; }
    }
}