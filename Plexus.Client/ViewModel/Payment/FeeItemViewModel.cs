using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Payment
{
    public class CreateFeeItemViewModel
    {
        [JsonProperty("feeGroupId")]
        public Guid FeeGroupId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name => Localizations?.GetDefault().Name;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<FeeItemLocalizationViewModel>? Localizations { get; set; }
    }

    public class FeeItemViewModel : CreateFeeItemViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("feeGroupName")]
        public string FeeGroupName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class FeeItemLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}