using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Research
{
    public class CreateResearchTemplateDTO
    {
        public IEnumerable<ResearchTemplateLocalizationDTO>? Localizations { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<CreateResearchTemplateSequenceDTO>? Sequences { get; set; }
    }

    public class ResearchTemplateDTO : CreateResearchTemplateDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class ResearchTemplateLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}