using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Registration;

namespace Plexus.Client.ViewModel.Registration
{
    public class BasePrerequisiteViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("condition")]
        public IEnumerable<PrerequisiteConditionViewModel> Condition { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("deactivatedAt")]
        public DateTime? DeactivatedAt { get; set; }
    }

    public class CreatePrerequisiteViewModel
    {
        [JsonProperty("curriculumVersionIds")]
        public IEnumerable<Guid> CurriculumVersionsIds { get; set; }

        [JsonProperty("prerequisiteConditions")]
        public IEnumerable<CreatePrerequisiteConditionViewModel> PrerequisiteConditions { get; set; }
    }

    public class CreatePrerequisiteConditionViewModel
    {
        [JsonProperty("conditionType")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(PrerequisiteCondition))]
        public PrerequisiteCondition? ConditionType { get; set; }

        [JsonProperty("conditions")]
        public IEnumerable<CreatePrerequisiteConditionDetailViewModel> Conditions { get; set; }
    }

    public class CreatePrerequisiteConditionDetailViewModel
    {
        [JsonProperty("prerequisiteType")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(PrerequisiteConditionType))]
        public PrerequisiteConditionType PrerequisiteType { get; set; }

        [JsonProperty("courseId")]
        public Guid? CourseId { get; set; }

        [JsonProperty("gradeId")]
        public Guid? GradeId { get; set; }

        [JsonProperty("gpa")]
        public decimal? GPA { get; set; }

        [JsonProperty("credit")]
        public decimal? Credit { get; set; }

        [JsonProperty("term")]
        public int? Term { get; set; }

        [JsonProperty("fromBatch")]
        public int? FromBatch { get; set; }

        [JsonProperty("toBatch")]
        public int? ToBatch { get; set; }

        [JsonProperty("prerequisiteConditions")]
        public IEnumerable<CreatePrerequisiteConditionViewModel>? PrerequisiteConditions { get; set; }
    }

    public class PrerequisiteViewModel
    {
        [JsonProperty("curriculumVersionNames")]
        public IEnumerable<string>? CurriculumVersionNames { get; set; }

        [JsonProperty("prerequisiteConditions")]
        public IEnumerable<PrerequisiteConditionViewModel> PrerequisiteConditions { get; set; }
    }

    public class PrerequisiteConditionViewModel
    {
        [JsonProperty("conditionType")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(PrerequisiteCondition))]
        public PrerequisiteCondition? ConditionType { get; set; }

        [JsonProperty("prerequisiteConditions")]
        public IEnumerable<PrerequisiteConditionDetailViewModel>? Conditions { get; set; }
    }

    public class PrerequisiteConditionDetailViewModel
    {
        [JsonProperty("prerequisiteType")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(PrerequisiteConditionType))]
        public PrerequisiteConditionType PrerequisiteType { get; set; }

        [JsonProperty("courseId")]
        public Guid? CourseId { get; set; }

        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("gradeId")]
        public Guid? GradeId { get; set; }

        [JsonProperty("gradeLetter")]
        public string? GradeLetter { get; set; }

        [JsonProperty("gpa")]
        public decimal? GPA { get; set; }

        [JsonProperty("credit")]
        public decimal? Credit { get; set; }

        [JsonProperty("term")]
        public int? Term { get; set; }

        [JsonProperty("fromBatch")]
        public int? FromBatch { get; set; }

        [JsonProperty("toBatch")]
        public int? ToBatch { get; set; }

        [JsonProperty("prerequisiteConditions")]
        public IEnumerable<PrerequisiteConditionViewModel>? PrerequisiteConditions { get; set; }
    }
}