using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateAcademicLevelViewModel
    {
        [JsonIgnore]
        public string Name => Localizations?.GetDefault().Name;

        [JsonIgnore]
        public string FormalName => Localizations?.GetDefault().FormalName;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<AcademicLevelLocalizationViewModel>? Localizations { get; set; }
    }

    public class AcademicLevelViewModel : CreateAcademicLevelViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }

        [JsonProperty("formalName")]
        public new string FormalName { get { return base.FormalName; } }
    }

    public class AcademicLevelLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("formalName")]
        public string? FormalName { get; set; }
    }
}