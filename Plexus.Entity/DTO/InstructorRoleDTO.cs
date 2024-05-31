using Plexus.Database.Enum;

namespace Plexus.Entity.DTO
{
    public class CreateInstructorRoleDTO
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<InstructorRoleLocalizationDTO> Localizations { get; set; }
    }

    public class InstructorRoleDTO : CreateInstructorRoleDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class InstructorRoleLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}