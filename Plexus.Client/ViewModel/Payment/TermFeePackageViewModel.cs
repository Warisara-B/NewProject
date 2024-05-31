using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Payment;
using Plexus.Database.Enum;

namespace Plexus.Client.ViewModel.Payment
{
    public class CreateTermFeePackageViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TermFeePackageType Type { get; set; }

        [JsonProperty("items")]
        public IEnumerable<CreateTermFeeItemViewModel> Items { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class TermFeePackageViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TermFeePackageType Type { get; set; }

        [JsonProperty("items")]
        public IEnumerable<TermFeeItemViewModel> Items { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class TermFeePackageLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}