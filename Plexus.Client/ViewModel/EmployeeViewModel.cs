using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel
{
    public class CreateEmployeeViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("gender")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Gender Gender { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("race")]
        public string Race { get; set; }

        [JsonProperty("religion")]
        public string Religion { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("address")]
        public CreateEmployeeAddressViewModel? Address { get; set; }

        [JsonProperty("status")]
        public CreateEmployeeWorkStatusViewModel? Status { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<EmployeeLocalizationViewModel>? Localizations { get; set; }
    }

    public class EmployeeViewModel : CreateEmployeeViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("cardImageUrl")]
        public string? CardImageUrl { get; set; }

        [JsonProperty("address")]
        public new EmployeeAddressViewModel? Address { get; set; }

        [JsonProperty("status")]
        public new EmployeeWorkStatusViewModel? Status { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class EmployeeLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }
    }

    public class CreateEmployeeAddressViewModel
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("district")]
        public string? District { get; set; }

        [JsonProperty("subDistrict")]
        public string? SubDistrict { get; set; }

        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("phoneNumber2")]
        public string? PhoneNumber2 { get; set; }

        [JsonProperty("emailAddress")]
        public string? EmailAddress { get; set; }

        [JsonProperty("personalEmailAddress")]
        public string? PersonalEmailAddress { get; set; }
    }

    public class EmployeeAddressViewModel : CreateEmployeeAddressViewModel
    {
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateEmployeeWorkStatusViewModel
    {
        [JsonProperty("academicLevelIds")]
        public IEnumerable<Guid>? AcademicLevelIds { get; set; }

        [JsonProperty("facultyId")]
        public Guid FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid DepartmentId { get; set; }

        [JsonProperty("employeeGroupId")]
        public Guid? EmployeeGroupId { get; set; }

        [JsonProperty("officeRoom")]
        public string OfficeRoom { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }
    }

    public class EmployeeWorkStatusViewModel : CreateEmployeeWorkStatusViewModel
    {
        [JsonProperty("academicLevels")]
        public IEnumerable<EmployeeAcademicLevelViewModel>? AcademicLevels { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("typeName")]
        public string? TypeName { get; set; }

        [JsonProperty("employeeGroupName")]
        public string? EmployeeGroupName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class EmployeeAcademicLevelViewModel
    {
        public Guid Id { get; set; }

        public string? NameEn { get; set; }
    }

    public class EmployeeInformationViewModel : UpdateEmployeeGeneralInformationViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("academicPositionName")]
        public string? AcademicPositionName { get; set; }

        [JsonProperty("careerPositionName")]
        public string? CareerPositionName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("workInformation")]
        public EmployeeWorkInformationViewModel? WorkInformation { get; set; }

        [JsonProperty("educationalBackgrounds")]
        public IEnumerable<EmployeeEducationalBackgroundViewModel>? EducationalBackgrounds { get; set; }
    }

    public class UpdateEmployeeGeneralInformationViewModel
    {
        [JsonProperty("academicPositionId")]
        public Guid? AcademicPositionId { get; set; }

        [JsonProperty("careerPositionId")]
        public Guid? CareerPositionId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName => Localizations?.GetDefault().FirstName;

        [JsonProperty("middleName")]
        public string? MiddleName => Localizations?.GetDefault().MiddleName;

        [JsonProperty("lastName")]
        public string? LastName => Localizations?.GetDefault().LastName;

        [JsonProperty("gender")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("nationality")]
        public string? Nationality { get; set; }

        [JsonProperty("religion")]
        public string? Religion { get; set; }

        [JsonProperty("race")]
        public string? Race { get; set; }

        [JsonProperty("citizenNo")]
        public string? CitizenNo { get; set; }

        [JsonProperty("universityEmail")]
        public string? UniversityEmail { get; set; }

        [JsonProperty("personalEmail")]
        public string? PersonalEmail { get; set; }

        [JsonProperty("alternativeEmail")]
        public string? AlternativeEmail { get; set; }

        [JsonProperty("phoneNumber1")]
        public string? PhoneNumber1 { get; set; }

        [JsonProperty("phoneNumber2")]
        public string? PhoneNumber2 { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<EmployeeLocalizationViewModel>? Localizations { get; set; }
    }

    public class EmployeeWorkInformationViewModel : UpdateEmployeeWorkInformationViewModel
    {
        [JsonProperty("facultyName")]
        public string FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string DepartmentName { get; set; }

        [JsonProperty("employeeGroupName")]
        public string? EmployeeGroupName { get; set; }
    }

    public class UpdateEmployeeWorkInformationViewModel
    {
        [JsonProperty("facultyId")]
        public Guid FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid DepartmentId { get; set; }

        [JsonProperty("employeeGroupId")]
        public Guid? EmployeeGroupId { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(EmployeeType))]
        public EmployeeType Type { get; set; }

        [JsonProperty("officeRoom")]
        public string? OfficeRoom { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("employeeExpertises")]
        public IEnumerable<EmployeeExpertiseViewModel>? EmployeeExpertises { get; set; }
    }

    public class EmployeeExpertiseViewModel
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(ExpertiseType))]
        public ExpertiseType Type { get; set; }

        [JsonProperty("major")]
        public string Major { get; set; }

        [JsonProperty("minor")]
        public string Minor { get; set; }
    }

    public class EmployeeEducationalBackgroundViewModel
    {
        [JsonProperty("institute")]
        public string Institute { get; set; }

        [JsonProperty("degreeLevel")]
        public string DegreeLevel { get; set; }

        [JsonProperty("degreeName")]
        public string DegreeName { get; set; }

        [JsonProperty("branch")]
        public string? Branch { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }
    }
}