using Plexus.Database.Enum;

namespace Plexus.Entity.DTO
{
    public class CreateEmployeeDTO
    {
        public string Title { get; set; }

        public string? Code { get; set; }

        public string FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public string Nationality { get; set; }

        public string Race { get; set; }

        public string Religion { get; set; }

        public bool IsActive { get; set; }

        public CreateEmployeeAddressDTO? Address { get; set; }

        public CreateEmployeeWorkStatusDTO? Status { get; set; }

        public IEnumerable<EmployeeLocalizationDTO>? Localizations { get; set; }
    }

    public class CreateEmployeeAddressDTO
    {
        public string Address { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string? District { get; set; }

        public string? SubDistrict { get; set; }

        public string? State { get; set; }

        public string? City { get; set; }

        public string PostalCode { get; set; }

        public string PhoneNumber { get; set; }

        public string? PhoneNumber2 { get; set; }

        public string? EmailAddress { get; set; }

        public string? PersonalEmailAddress { get; set; }
    }

    public class CreateEmployeeWorkStatusDTO
    {
        public IEnumerable<Guid>? AcademicLevelIds { get; set; }

        public Guid FacultyId { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid? EmployeeGroupId { get; set; }

        public string OfficeRoom { get; set; }

        public string Remark { get; set; }
    }

    public class EmployeeAddressDTO : CreateEmployeeAddressDTO
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class EmployeeWorkStatusDTO : CreateEmployeeWorkStatusDTO
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class EmployeeDTO : CreateEmployeeDTO
    {
        public Guid Id { get; set; }

        public string? CardImagePath { get; set; }

        public new EmployeeAddressDTO? Address { get; set; }

        public new EmployeeWorkStatusDTO? Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class EmployeeLocalizationDTO
    {
        public LanguageCode Language { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }
    }
}