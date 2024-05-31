using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("CurriculumInstructors")]
    public class CurriculumInstructor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CurriculumVersionId { get; set; }

        public Guid InstructorId { get; set; }

        public Guid InstructorRoleId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(CurriculumVersionId))]
        public virtual CurriculumVersion CurriculumVersion { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public virtual Employee Instructor { get; set; }

        [ForeignKey(nameof(InstructorRoleId))]
        public virtual InstructorRole InstructorRole { get; set; }
    }
}