namespace Plexus.Entity.DTO.Academic
{
    public class UpdateCourseTrackDetailDTO
    {
        public Guid CourseTrackId { get; set; }
        
        public Guid CourseId { get; set; }

        public int Step { get; set; }

        public bool IsRequired { get; set; }
    }
}