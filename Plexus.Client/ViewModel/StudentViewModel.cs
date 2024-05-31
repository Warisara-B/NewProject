using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Student;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel
{
    public class CreateStudentViewModel
    {
        [JsonProperty("academicLevelId")]
        public Guid AcademicLevelId { get; set; }

        [JsonProperty("facultyId")]
        public Guid FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("curriculumVersionId")]
        public Guid CurriculumVersionId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("gender")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("birthCountry")]
        public string? BirthCountry { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("religion")]
        public string Religion { get; set; }

        [JsonProperty("race")]
        public string Race { get; set; }

        [JsonProperty("citizenId")]
        public string? CitizenId { get; set; }

        [JsonProperty("passports")]
        public virtual IEnumerable<CreatePassportViewModel>? Passports { get; set; }

        [JsonProperty("deformations")]
        public virtual IEnumerable<CreateDeformationViewModel>? Deformations { get; set; }

        [JsonProperty("studentStatus")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(AcademicStatus))]
        public AcademicStatus StudentStatus { get; set; }

        [JsonProperty("studentStatusEffectiveDate")]
        public DateTime StudentStatusEffectiveDate { get; set; }

        [JsonProperty("studentStatusRemark")]
        public string? StudentStatusRemark { get; set; }

        [JsonProperty("bankBranch")]
        public string? BankBranch { get; set; }

        [JsonProperty("bankAccountNo")]
        public string? BankAccountNo { get; set; }

        [JsonProperty("bankAccountUpdatedAt")]
        public DateTime? BankAccountUpdatedAt { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("universityEmail")]
        public string? UniversityEmail { get; set; }

        [JsonProperty("personalEmail")]
        public string? PersonalEmail { get; set; }

        [JsonProperty("alternativeEmail")]
        public string? AlternativeEmail { get; set; }

        [JsonProperty("facebook")]
        public string? Facebook { get; set; }

        [JsonProperty("line")]
        public string? Line { get; set; }

        [JsonProperty("other")]
        public string? Other { get; set; }

        [JsonProperty("phoneNumber1")]
        public string? PhoneNumber1 { get; set; }

        [JsonProperty("phoneNumber2")]
        public string? PhoneNumber2 { get; set; }

        [JsonProperty("batchCod")]
        public int BatchCode { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<StudentLocalizationViewModel>? Localizations { get; set; }
    }

    public class StudentViewModel : CreateStudentViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("academicStatus")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(AcademicStatus))]
        public AcademicStatus AcademicStatus { get; set; }

        [JsonProperty("cardImageUrl")]
        public string? CardImageUrl { get; set; }

        [JsonProperty("gpa")]
        public decimal? GPA { get; set; }

        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("curriculumVersionName")]
        public string? CurriculumVersionName { get; set; }

        [JsonProperty("studentFeeTypeName")]
        public string? StudentFeeTypeName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class StudentLocalizationViewModel
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

    public class CreateStudentGeneralInfoViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("firstName")]
        public string FirstName => Localizations?.GetDefault().FirstName;

        [JsonProperty("middleName")]
        public string? MiddleName => Localizations?.GetDefault().MiddleName;

        [JsonProperty("lastName")]
        public string LastName => Localizations?.GetDefault().LastName;

        [JsonProperty("gender")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("birthCountry")]
        public string? BirthCountry { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("religion")]
        public string Religion { get; set; }

        [JsonProperty("race")]
        public string Race { get; set; }

        [JsonProperty("citizenId")]
        public string? CitizenId { get; set; }

        [JsonProperty("passports")]
        public IEnumerable<CreatePassportViewModel> Passports { get; set; }

        [JsonProperty("deformations")]
        public IEnumerable<CreateDeformationViewModel> Deformations { get; set; }

        [JsonProperty("studentStatus")]
        public StudentStatusViewModel? StudentStatus { get; set; }

        [JsonProperty("bankBranch")]
        public string? BankBranch { get; set; }

        [JsonProperty("bankAccountNo")]
        public string? BankAccountNo { get; set; }

        [JsonProperty("bankAccountUpdatedAt")]
        public DateTime? BankAccountUpdatedAt { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<StudentLocalizationViewModel>? Localizations { get; set; }
    }

    public class StudentGeneralInfoViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("firstName")]
        public string FirstName => Localizations?.GetDefault().FirstName;

        [JsonProperty("middleName")]
        public string? MiddleName => Localizations?.GetDefault().MiddleName;

        [JsonProperty("lastName")]
        public string LastName => Localizations?.GetDefault().LastName;

        [JsonProperty("gender")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [JsonProperty("birthDate")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("birthCountry")]
        public string? BirthCountry { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("religion")]
        public string Religion { get; set; }

        [JsonProperty("race")]
        public string Race { get; set; }

        [JsonProperty("citizenId")]
        public string? CitizenId { get; set; }

        [JsonProperty("passports")]
        public IEnumerable<CreatePassportViewModel> Passports { get; set; }

        [JsonProperty("deformations")]
        public IEnumerable<CreateDeformationViewModel> Deformations { get; set; }

        [JsonProperty("studentStatus")]
        public IEnumerable<StudentStatusViewModel>? StudentStatus { get; set; }

        [JsonProperty("bankBranch")]
        public string? BankBranch { get; set; }

        [JsonProperty("bankAccountNo")]
        public string? BankAccountNo { get; set; }

        [JsonProperty("bankAccountUpdatedAt")]
        public DateTime? BankAccountUpdatedAt { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<StudentLocalizationViewModel>? Localizations { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class StudentStatusViewModel
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(AcademicStatus))]
        public AcademicStatus Status { get; set; }

        [JsonProperty("effectiveDate")]
        public DateTime EffectiveDate { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class StudentCardViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("cardImageUrl")]
        public string? CardImageUrl { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("firstName")]
        public string FirstName => Localizations?.GetDefault().FirstName;

        [JsonProperty("middleName")]
        public string? MiddleName => Localizations?.GetDefault().MiddleName;

        [JsonProperty("lastName")]
        public string LastName => Localizations?.GetDefault().LastName;

        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("curriculumVersionName")]
        public string? CurriculumVersionName { get; set; }

        [JsonProperty("gpax")]
        public decimal? GPAX { get; set; }

        [JsonProperty("completedCredit")]
        public decimal? CompletedCredit { get; set; }

        [JsonProperty("studyPlanName")]
        public string? StudyPlanName { get; set; }

        [JsonProperty("advisor")]
        public AdvisorViewModel? Advisor { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("studentStatuses")]
        public IEnumerable<StudentStatusViewModel> StudentStatuses { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<StudentLocalizationViewModel>? Localizations { get; set; }
    }

    public class AdvisorViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("firstName")]
        public string FirstName => Localizations?.GetDefault().FirstName;

        [JsonProperty("middleName")]
        public string? MiddleName => Localizations?.GetDefault().MiddleName;

        [JsonProperty("lastName")]
        public string LastName => Localizations?.GetDefault().LastName;

        [JsonProperty("localizations")]
        public IEnumerable<EmployeeLocalizationViewModel>? Localizations { get; set; }
    }

    public class CreateStudentContactViewModel
    {
        [JsonProperty("universityEmail")]
        public string? UniversityEmail { get; set; }

        [JsonProperty("personalEmail")]
        public string? PersonalEmail { get; set; }

        [JsonProperty("alternativeEmail")]
        public string? AlternativeEmail { get; set; }

        [JsonProperty("facebook")]
        public string? Facebook { get; set; }

        [JsonProperty("line")]
        public string? Line { get; set; }

        [JsonProperty("other")]
        public string? Other { get; set; }

        [JsonProperty("phoneNumber1")]
        public string? PhoneNumber1 { get; set; }

        [JsonProperty("phoneNumber2")]
        public string? PhoneNumber2 { get; set; }
    }

    public class StudentContactViewModel : CreateStudentContactViewModel
    {
        [JsonProperty("studentId")]
        public Guid Id { get; set; }
    }

    public class CreateStudentAcademicInfoViewModel
    {
        [JsonProperty("academicLevelId")]
        public Guid? AcademicLevelId { get; set; }

        [JsonProperty("academicProgramId")]
        public Guid? AcademicProgramId { get; set; }

        [JsonProperty("previousCode")]
        public string? PreviousCode { get; set; }

        [JsonProperty("studentCode")]
        public string? StudentCode { get; set; }

        [JsonProperty("batch")]
        public int? Batch { get; set; }
    }

    public class StudentAcademicInfoViewModel : CreateStudentAcademicInfoViewModel
    {
        [JsonProperty("studentId")]
        public Guid StudentId { get; set; }

        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("academicProgramName")]
        public string? AcademicProgramName { get; set; }
    }

    public class CreateStudentCurriculumViewModel
    {
        [JsonProperty("facultyId")]
        public Guid FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid DepartmentId { get; set; }

        [JsonProperty("curriculumVersionId")]
        public Guid CurriculumVersionId { get; set; }

        [JsonProperty("studyPlanId")]
        public Guid StudyPlanId { get; set; }
    }

    public class StudentCurriculumViewModel : CreateStudentCurriculumViewModel
    {
        [JsonProperty("studentId")]
        public Guid StudentId { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("curriculumVersionName")]
        public string? CurriculumVersionName { get; set; }

        [JsonProperty("studyPlanName")]
        public string? StudyPlanName { get; set; }
    }
}

