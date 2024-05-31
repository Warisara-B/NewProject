using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic.Curriculum
{
    [Table("StudyPlanDetails")]
    public class StudyPlanDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StudyPlanId { get; set; }

        public Guid CourseId { get; set; }

        public int Year { get; set; }

        public string Term { get; set; }

        [ForeignKey(nameof(StudyPlanId))]
        public virtual StudyPlan StudyPlan { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }
    }
}