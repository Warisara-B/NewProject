using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;

namespace Plexus.Database.Model.Localization
{
    [Table("StudentGuardianLocalizations", Schema = "localization")]
    public class StudentGuardianLocalization
    {
        public Guid StudentGuardianId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public LanguageCode Language { get; set; }

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        [ForeignKey(nameof(StudentGuardianId))]
        public virtual StudentGuardian StudentGuardian { get; set; }
    }
}