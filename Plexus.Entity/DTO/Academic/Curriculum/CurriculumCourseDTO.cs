using System;
namespace Plexus.Entity.DTO.Academic.Curriculum
{
    public class CurriculumCourseDTO : CurriculumCourseGroupIgnoreCourseDTO
    {
        public Guid? RequiredGradeId { get; set; }
    }
}

