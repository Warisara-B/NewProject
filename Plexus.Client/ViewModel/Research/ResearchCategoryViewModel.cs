using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Research
{
    public class CreateResearchCategoryViewModel
    {
        [JsonIgnore]
        public string Name => Localizations.GetDefault().Name;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<ResearchCategoryLocalizationViewModel> Localizations { get; set; }
    }

    public class ResearchCategoryViewModel : CreateResearchCategoryViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }
    }

    public class ResearchCategoryLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]

        public LanguageCode Language { get; set; }
        [JsonProperty("name")]

        public string? Name { get; set; }
    }
}