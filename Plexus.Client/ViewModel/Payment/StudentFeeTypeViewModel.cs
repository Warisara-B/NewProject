using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;

namespace Plexus.Client.ViewModel.Payment
{
    public class StudentFeeTypeViewModel : CreateStudentFeeTypeViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateStudentFeeTypeViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<StudentFeeTypeLocalizationViewModel>? Localizations { get; set; }
    }

    public class StudentFeeTypeLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}