using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Academic
{
    public class CreateTeachingTypeDTO
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<TeachingTypeLocalizationDTO>? Localizations { get; set; }
    }

    public class TeachingTypeDTO : CreateTeachingTypeDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class TeachingTypeLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}