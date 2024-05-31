using Plexus.Database.Enum;
using Plexus.Database.Enum.Payment;

namespace Plexus.Entity.DTO.Payment
{
    public class CreateTermFeePackageDTO
    {
        public string Name { get; set; }

        public TermFeePackageType Type { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<TermFeePackageLocalizationDTO> Localizations { get; set; }
    }

    public class TermFeePackageDTO : CreateTermFeePackageDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class TermFeePackageLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}