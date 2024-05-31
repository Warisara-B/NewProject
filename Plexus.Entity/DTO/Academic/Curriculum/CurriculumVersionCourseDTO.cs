namespace Plexus.Entity.DTO.Academic.Curriculum
{
    public class CurriculumVersionCourseDTO : CurriculumCourseDTO
	{
        public new Guid? CourseGroupId { get; set; }

        public Guid? SpecialzationId { get; set; }
	}
}

