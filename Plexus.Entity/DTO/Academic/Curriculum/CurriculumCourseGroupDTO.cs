using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic.Curriculum;

namespace Plexus.Entity.DTO.Academic.Curriculum
{
    public class CreateCurriculumCourseGroupDTO
    {
        public Guid CurriculumVersionId { get; set; }

        public Guid? ParentCourseGroupId { get; set; }

        public string Name { get; set; }

        public CourseGroupType Type { get; set; }

        public string? Description { get; set; }

        public int Sequence { get; set; }

        public decimal RequiredCredit { get; set; }

        public string? Remark { get; set; }

        public IEnumerable<CurriculumCourseGroupLocalizationDTO> Localizations { get; set; }
    }

    public class CurriculumCourseGroupDTO : CreateCurriculumCourseGroupDTO
    {
        public Guid Id { get; set; }

        public IEnumerable<CurriculumCourseDTO> Courses { get; set; }

        public IEnumerable<CurriculumCourseGroupIgnoreCourseDTO> IgnoreCourses { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CurriculumCourseGroupLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}

