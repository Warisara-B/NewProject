using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateTeachingTypeViewModel
    {
        [JsonIgnore]
        public string? Name => Localizations?.GetDefault().Name;

        [JsonIgnore]
        public string? Description => Localizations?.GetDefault().Description;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<TeachingTypeLocalizationViewModel>? Localizations { get; set; }
    }

    public class TeachingTypeViewModel : CreateTeachingTypeViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }

        [JsonProperty("description")]
        public new string Description { get { return base.Description; } }
    }

    public class TeachingTypeLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}