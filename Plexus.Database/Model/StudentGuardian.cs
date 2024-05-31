using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum;
using Plexus.Database.Model.Localization;

namespace Plexus.Database.Model
{
    [Table("StudentGuardians")]
    public class StudentGuardian
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public Relationship Relationship { get; set; }

        public string? CitizenNo { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EmailAddress { get; set; }

        public bool IsMainContact { get; set; }

        public bool IsEmergencyContact { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        public virtual IEnumerable<StudentGuardianLocalization> Localizations { get; set; }
    }
}

