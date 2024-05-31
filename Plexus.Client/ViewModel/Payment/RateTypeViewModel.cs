using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;
using ServiceStack;

namespace Plexus.Client.ViewModel.Payment
{
    public class CreateRateTypeViewModel
    {
        [JsonProperty("name")]
        public string Name => Localizations?.GetDefault().Name;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<RateTypeLocalizationViewModel>? Localizations { get; set; }
    }

    public class RateTypeViewModel : CreateRateTypeViewModel
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

    public class RateTypeLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}