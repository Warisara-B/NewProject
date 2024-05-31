using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic.Curriculum;

namespace Plexus.Entity.DTO.Academic.Curriculum
{
    public class CreateAcademicSpecializationDTO
    {
        public Guid? ParentAcademicSpecializationId { get; set; }

        public string Name { get; set; }

		public string Code { get; set; }

		public string? Abbreviation { get; set; }

		public SpecializationType Type { get; set; }

        public string? Description { get; set; }

		public decimal RequiredCredit { get; set; }

		public string? Remark { get; set; }

		public int Level { get; set; }

        public IEnumerable<AcademicSpecializationLocalizationDTO> Localizations { get; set; }
    }

    public class AcademicSpecializationDTO : CreateAcademicSpecializationDTO
    {
        public Guid Id { get; set; }

        public IEnumerable<SpecializationCourseDTO> Courses { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class AcademicSpecializationLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Abbreviation { get; set; }
    }
}