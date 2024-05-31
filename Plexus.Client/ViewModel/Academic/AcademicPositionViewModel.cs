using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateAcademicPositionViewModel
    {
        [JsonProperty("abbreviation")]
        public string Abbreviation => Localizations?.GetDefault().Abbreviation;

        [JsonProperty("name")]
        public string Name => Localizations?.GetDefault().Name;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<AcademicPositionLocalizationViewModel>? Localizations { get; set; }
    }

    public class AcademicPositionViewModel : CreateAcademicPositionViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("abbreviation")]
        public new string Abbreviation { get { return base.Abbreviation; } }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class AcademicPositionLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}