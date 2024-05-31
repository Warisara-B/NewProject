using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Research;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Research
{
    public class UpsertResearchTemplateViewModel
    {
        [JsonProperty("localizations")]
        public IEnumerable<ResearchTemplateLocalizationViewModel>? Localizations { get; set; }

        [JsonIgnore]
        public string? Name => Localizations?.GetDefault().Name;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("sequences")]
        public IEnumerable<UpsertResearchTemplateSequenceViewModel>? Sequences { get; set; }
    }

    public class ResearchTemplateViewModel : UpsertResearchTemplateViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public new string Name { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class ResearchTemplateLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public class UpsertResearchTemplateSequenceViewModel
    {
        [JsonIgnore]
        public string Name => Localizations?.GetDefault().Name;

        [JsonProperty("filePrefix")]
        public string FilePrefix { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(ResearchSequenceType))]
        public ResearchSequenceType Type { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<ResearchTemplateSequenceLocalizationViewModel>? Localizations { get; set; }
    }

    public class ResearchTemplateSequenceViewModel : UpsertResearchTemplateSequenceViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public new string Name { get; set; }
    }

    public class ResearchTemplateSequenceLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public class ResearchTemplateListViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}