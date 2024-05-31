using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic.Curriculum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CreateAcademicSpecializationViewModel
    {
        [JsonProperty("parentAcademicSpecializationId")]
        public Guid? ParentAcademicSpecializationId { get; set; }

        [JsonIgnore]
        public string Name => Localizations?.GetDefault().Name;

        [JsonIgnore]
        public string? Abbreviation => Localizations?.GetDefault().Abbreviation;

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonIgnore]
        public string? Description => Localizations?.GetDefault().Description;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SpecializationType Type { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("requiredCredit")]
        public decimal RequiredCredit { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; } = string.Empty;

        [JsonProperty("localizations")]
        public IEnumerable<AcademicSpecializationLocalizationViewModel>? Localizations { get; set; }
    }

    public class AcademicSpecializationViewModel : CreateAcademicSpecializationViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("subGroups")]
        public IEnumerable<AcademicSpecializationViewModel>? SubGroups { get; set; }

        [JsonProperty("courses")]
        public IEnumerable<SpecializationCourseViewModel>? Courses { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string? Name { get { return base.Name; } }

        [JsonProperty("abbreviation")]
        public new string? Abbreviation { get { return base.Abbreviation; } }

        [JsonProperty("description")]
        public new string? Description { get { return base.Description; } }
    }

    public class AcademicSpecializationLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("abbreviation")]
        public string? Abbreviation { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}