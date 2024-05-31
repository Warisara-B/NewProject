using Plexus.Database.Enum;

namespace Plexus.Entity.DTO.Payment
{
    public class StudentFeeTypeDTO : CreateStudentFeeTypeDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CreateStudentFeeTypeDTO
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<StudentFeeTypeLocalizationDTO> Localizations { get; set; }
    }

    public class StudentFeeTypeLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }
}