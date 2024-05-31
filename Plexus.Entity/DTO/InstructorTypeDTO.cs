using Plexus.Database.Enum;

namespace Plexus.Entity.DTO
{
    public class CreateInstructorTypeDTO
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<InstructorTypeLocalizationDTO> Localizations { get; set; }
    }

    public class InstructorTypeDTO : CreateInstructorTypeDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class InstructorTypeLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}