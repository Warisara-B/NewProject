namespace Plexus.Entity.DTO.Academic.Curriculum
{
    public class CreateEquivalentCourseDTO
    {
        public Guid CourseId { get; set; }

        public Guid EquivalentCourseId { get; set; }
    }

    public class EquivalentCourseDTO : CreateEquivalentCourseDTO
    {
        public Guid CurriculumVersionId { get; set; }
    }
}