using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Student;

namespace Plexus.Service.ViewModel
{
    public class StudentProfileViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("profileImageUrl")]
        public string? ProfileImageUrl { get; set; }

        [JsonProperty("faculty")]
        public StudentFacultyInformationViewModel Faculty { get; set; }
    }

    public class StudentProfileCardViewModel : StudentProfileViewModel
    {
        [JsonProperty("gpax")]
        public decimal? GPAX { get; set; }

        [JsonProperty("completedCredit")]
        public decimal? CompletedCredit { get; set; }
    }

    public class StudentFacultyInformationViewModel
    {
        [JsonProperty("logoUrl")]
        public string? LogoUrl { get; set; }

        [JsonProperty("name")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmenName")]
        public string? DepartmentName { get; set; }
    }

    public class StudentFullProfileViewModel : StudentProfileViewModel
    {
        [JsonProperty("informations")]
        public IEnumerable<StudentInformationViewModel>? Informations { get; set; } = Enumerable.Empty<StudentInformationViewModel>();

        [JsonProperty("contactPersons")]
        public IEnumerable<StudentContactPersonViewModel>? ContactPersons { get; set; } = Enumerable.Empty<StudentContactPersonViewModel>();
    }

    public class StudentInformationViewModel
    {
        [JsonProperty("key")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(StudentInformationKey))]
        public StudentInformationKey Key { get; set; }

        [JsonProperty("value")]
        public string? Value { get; set; }
    }

    public class StudentContactPersonViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("relationship")]
        public Relationship? Relationship { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }
    }
}