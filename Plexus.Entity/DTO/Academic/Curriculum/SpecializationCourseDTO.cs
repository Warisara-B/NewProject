namespace Plexus.Entity.DTO.Academic.Curriculum
{
    public class SpecializationCourseDTO
    {
        public Guid AcademicSpecializationId { get; set; }

		public Guid CourseId { get; set; }

		public Guid? RequiredGradeId { get; set; }

		public bool IsRequiredCourse { get; set; }
    }
}