using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Payment
{
    public class CreateRateTypeDTO
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<RateTypeLocalizationDTO> Localizations { get; set; }
    }

    public class RateTypeDTO : CreateRateTypeDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class RateTypeLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}