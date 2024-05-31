using Plexus.Database.Enum;

namespace Plexus.Entity.DTO
{
    public class CreateStudentGuardianDTO
    {
        public Guid StudentId { get; set; }

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        public Relationship Relationship { get; set; }

        public string? CitizenNo { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EmailAddress { get; set; }

        public bool IsMainContact { get; set; }

        public bool IsEmergencyContact { get; set; }

        public IEnumerable<StudentGuardianLocalizationDTO>? Localizations { get; set; }
    }

    public class StudentGuardianDTO : CreateStudentGuardianDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class StudentGuardianLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }
    }
}