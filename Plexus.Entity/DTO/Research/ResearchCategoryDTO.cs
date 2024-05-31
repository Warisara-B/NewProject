using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Research
{
    public class CreateResearchCategoryDTO
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<ResearchCategoryLocalizationDTO> Localizations { get; set; }
    }

    public class ResearchCategoryDTO : CreateResearchCategoryDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class ResearchCategoryLocalizationDTO : LocalizationDTO
    {
        public string? Name { get; set; }
    }
}