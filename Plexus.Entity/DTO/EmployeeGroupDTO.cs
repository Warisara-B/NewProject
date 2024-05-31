using Plexus.Database.Enum;

namespace Plexus.Entity.DTO
{
    public class CreateEmployeeGroupDTO
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<EmployeeGroupLocalizationDTO> Localizations { get; set; }
    }

    public class EmployeeGroupDTO : CreateEmployeeGroupDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class EmployeeGroupLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}