using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("StudyPlans")]
    public class StudyPlan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CurriculumVersionId { get; set; }

        public string Name { get; set; }

        [ForeignKey(nameof(CurriculumVersionId))]
        public virtual CurriculumVersion CurriculumVersion { get; set; }

        public virtual IEnumerable<StudyPlanDetail>? StudyPlanDetails { get; set; }
    }
}