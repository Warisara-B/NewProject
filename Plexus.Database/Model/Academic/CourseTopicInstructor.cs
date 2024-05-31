using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic
{
    [Table("CourseTopicInstructors")]
    public class CourseTopicInstructor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CourseTopicId { get; set; }

        public Guid InstructorId { get; set; }

        [ForeignKey(nameof(CourseTopicId))]
        public CourseTopic CourseTopic { get; set; }

        [ForeignKey(nameof(InstructorId))]
        public Employee Instructor { get; set; }
    }
}