using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Academic.Curriculum;

namespace Plexus.Database.Model.Registration
{
    [Table("CurriculumCoursePrerequisites")]
    public class CurriculumCoursePrerequisite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CurriculumVersionId { get; set; }

        public Guid CoursePrerequisiteId { get; set; }

        [ForeignKey(nameof(CurriculumVersionId))]
        public virtual CurriculumVersion CurriculumVersion { get; set; }

        [ForeignKey(nameof(CoursePrerequisiteId))]
        public virtual CoursePrerequisite CoursePrerequisite { get; set; }
    }
}