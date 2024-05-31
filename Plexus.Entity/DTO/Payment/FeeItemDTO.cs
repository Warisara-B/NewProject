using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Payment
{
    public class CreateFeeItemDTO
    {
        public Guid FeeGroupId { get; set; }
        
        public string Code { get; set; }

        public string Name { get; set; }

        public string AccountCode { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<FeeItemLocalizationDTO> Localizations { get; set; }
    }

    public class FeeItemDTO : CreateFeeItemDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class FeeItemLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}