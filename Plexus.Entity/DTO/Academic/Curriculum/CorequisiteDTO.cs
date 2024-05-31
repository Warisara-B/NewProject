namespace Plexus.Entity.DTO.Academic.Curriculum
{
    public class CreateCorequisiteDTO
    {
        public Guid CourseId { get; set; }

        public Guid CorequisiteCourseId { get; set; }
    }

    public class CorequisiteDTO : CreateCorequisiteDTO
    {
        public Guid CurriculumVersionId { get; set; }
    }
}