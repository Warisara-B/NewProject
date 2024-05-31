using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel
{
    public class CreateEmployeeGroupViewModel
    {
        [JsonProperty("name")]
        public string Name => Localizations?.GetDefault().Name;

        [JsonProperty("localizations")]
        public IEnumerable<EmployeeGroupLocalizationViewModel>? Localizations { get; set; }
    }

    public class EmployeeGroupViewModel : CreateEmployeeGroupViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class EmployeeGroupLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}