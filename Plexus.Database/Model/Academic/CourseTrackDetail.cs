using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic
{
    [Table("CourseTrackDetails")]
    public class CourseTrackDetail
    {
        public Guid CourseTrackId { get; set; }

        public Guid CourseId { get; set; }

        public int Step { get; set; }

        public bool IsRequired { get; set; }

        [ForeignKey(nameof(CourseTrackId))]
        public virtual CourseTrack CourseTrack { get; set; }

        [ForeignKey(nameof(CourseId))]
        public virtual Course Course { get; set; }
    }
}