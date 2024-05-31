using Plexus.Database.Enum;
using Plexus.Database.Enum.Research;

namespace Plexus.Entity.DTO.Research
{
    public class CreateResearchTemplateSequenceDTO
    {
        public string? Name { get; set; }

        public string? FilePrefix { get; set; }

        public ResearchSequenceType Type { get; set; }

        public IEnumerable<ResearchTemplateSequenceLocalizationDTO>? Localizations { get; set; }
    }

    public class ResearchTemplateSequenceDTO : CreateResearchTemplateSequenceDTO
    {
        public Guid Id { get; set; }
    }

    public class ResearchTemplateSequenceLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}