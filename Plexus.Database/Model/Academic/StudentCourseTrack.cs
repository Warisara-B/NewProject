using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic
{
    [Table("StudentCourseTracks")]
    public class StudentCourseTrack
    {
        public Guid StudentId { get; set; }
        
        public Guid CourseTrackId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(CourseTrackId))]
        public virtual CourseTrack CourseTrack { get; set; }
    }   
}