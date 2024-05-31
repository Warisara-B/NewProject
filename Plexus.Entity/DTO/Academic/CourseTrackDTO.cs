using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Academic
{
    public class CreateCourseTrackDTO
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<CourseTrackLocalizationDTO> Localizations { get; set; }
    }

    public class CourseTrackDTO : CreateCourseTrackDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CourseTrackLocalizationDTO
    {
		public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}