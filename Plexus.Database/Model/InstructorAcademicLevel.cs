using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model
{
    [Table("InstructorAcademicLevels")]
    public class InstructorAcademicLevel
    {
        public Guid InstructorId { get; set; }

        public Guid AcademicLevelId { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public virtual Employee Instructor { get; set; }

        [ForeignKey(nameof(AcademicLevelId))]
        public virtual AcademicLevel AcademicLevel { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}