using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization;

namespace Plexus.Database.Model
{
    [Table("InstructorRoles")]
    public class InstructorRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? InstructorId { get; set; }

        public int Sequence { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public Employee? Instructor { get; set; }

        public IEnumerable<InstructorRoleLocalization> Localizations { get; set; }
    }
}