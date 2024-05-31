using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Model.Localization.Academic;

namespace Plexus.Database.Model.Academic
{
    [Table("CourseTopics")]
    public class CourseTopic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public Guid CourseId { get; set; }

        public decimal LectureHour { get; set; }

        public decimal LabHour { get; set; }

        public decimal OtherHour { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public string? MigrateReference { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }

        public virtual IEnumerable<CourseTopicInstructor> Instructors { get; set; }

        public virtual IEnumerable<CourseTopicLocalization> Localizations { get; set; }
    }
}

