using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Academic.Faculty;

namespace Plexus.Database.Model
{
    [Table("StudentCurriculumLogs")]
    public class StudentCurriculumLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public Guid FacultyId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid CurriculumVersionId { get; set; }

        public Guid StudyPlanId { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [ForeignKey(nameof(FacultyId))]
        public virtual Faculty Faculty { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department Department { get; set; }

        [ForeignKey(nameof(CurriculumVersionId))]
        public virtual CurriculumVersion CurriculumVersion { get; set; }

        [ForeignKey(nameof(StudyPlanId))]
        public virtual StudyPlan? StudyPlan { get; set; }
    }
}