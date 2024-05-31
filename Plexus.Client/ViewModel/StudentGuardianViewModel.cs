using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel
{
    public class CreateStudentGuardianViewModel
    {
        [JsonProperty("firstName")]
        public string FirstName => Localizations?.GetDefault().FirstName;

        [JsonProperty("middleName")]
        public string? MiddleName => Localizations?.GetDefault().MiddleName;

        [JsonProperty("lastName")]
        public string LastName => Localizations?.GetDefault().LastName;

        [JsonProperty("relationship")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(Relationship))]
        public Relationship Relationship { get; set; }

        [JsonProperty("citizenNo")]
        public string? CitizenNo { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("emailAddress")]
        public string? EmailAddress { get; set; }

        [JsonProperty("isMainContact")]
        public bool IsMainContact { get; set; }

        [JsonProperty("isEmergencyContact")]
        public bool IsEmergencyContact { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<StudentGuardianLocalizationViewModel>? Localizations { get; set; }
    }

    public class StudentGuardianViewModel : CreateStudentGuardianViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("studentId")]
        public Guid StudentId { get; set; }

        [JsonProperty("firstName")]
        public new string? FirstName { get { return base.FirstName; } }

        [JsonProperty("middleName")]
        public new string? MiddleName { get { return base.MiddleName; } }

        [JsonProperty("lastName")]
        public new string? LastName { get { return base.LastName; } }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class StudentGuardianLocalizationViewModel
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
}