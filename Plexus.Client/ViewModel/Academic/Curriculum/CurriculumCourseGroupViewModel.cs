using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic.Curriculum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CreateCurriculumCourseGroupViewModel
    {
        [JsonProperty("curriculumVersionId")]
        public Guid CurriculumVersionId { get; set; }

        [JsonProperty("parentCourseGroupId")]
        public Guid? ParentCourseGroupId { get; set; }

        [JsonIgnore]
        public string Name => Localizations?.GetDefault().Name;

        [JsonIgnore]
        public string? Description => Localizations?.GetDefault().Description;

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(CourseGroupType))]
        public CourseGroupType Type { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("requiredCredit")]
        public decimal RequiredCredit { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; } = string.Empty;

        [JsonProperty("localizations")]
        public IEnumerable<CurriculumCourseGroupLocalizationViewModel>? Localizations { get; set; }
    }

    public class CurriculumCourseGroupViewModel : CreateCurriculumCourseGroupViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("subGroups")]
        public IEnumerable<CurriculumCourseGroupViewModel>? SubGroups { get; set; }

        [JsonProperty("courses")]
        public IEnumerable<CurriculumCourseViewModel>? Courses { get; set; }

        [JsonProperty("ignoreCourses")]
        public IEnumerable<CurriculumCourseGroupIgnoreCourseViewModel>? IgnoreCourses { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string? Name { get { return base.Name; } }

        [JsonProperty("description")]
        public new string? Description { get { return base.Description; } }
    }

    public class CurriculumCourseGroupLocalizationViewModel
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

