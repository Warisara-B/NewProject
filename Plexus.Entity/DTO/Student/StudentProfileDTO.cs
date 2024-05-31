using Plexus.Database.Enum.Student;

namespace Plexus.Entity.DTO
{
    public class StudentProfileDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Code { get; set; }

        public string? ProfileImageUrl { get; set; }
    }

    public class StudentProfileCardDTO : StudentProfileDTO
    {
        public decimal? GPAX { get; set; }

        public decimal? CompletedCredit { get; set; }

    }

    public class StudentFullProfileDTO : StudentProfileDTO
    {
        public IEnumerable<StudentInformationDTO>? Informations { get; set; }

        public IEnumerable<StudentContactPersonDTO>? ContactPersons { get; set; }
    }

    public class StudentInformationDTO
    {
        public StudentInformationKey Key { get; set; }

        public string? Value { get; set; }
    }

    public class StudentContactPersonDTO
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string? Relationship { get; set; }

        public string? Address { get; set; }
    }
}