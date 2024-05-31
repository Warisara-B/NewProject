using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Academic
{
    public class CreateCourseTopicDTO
    {
        public string Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid CourseId { get; set; }
        public decimal LectureHour { get; set; }
        public decimal LabHour { get; set; }
        public decimal OtherHour { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<CourseTopicLocalizationDTO> Localizations { get; set; }
    }   

    public class CourseTopicDTO : CreateCourseTopicDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CourseTopicLocalizationDTO
    {
        public LanguageCode Language { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}