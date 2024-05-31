using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Payment;

namespace Plexus.Client.ViewModel.Payment
{
    public class CreateCourseRateViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("conditions")]
        public CreateCourseRateConditionViewModel? Conditions { get; set; }

        [JsonProperty("rateTypeId")]
        public Guid RateTypeId { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }

    public class CourseRateViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("conditions")]
        public CourseRateConditionViewModel? Conditions { get; set; }

        [JsonProperty("rateTypeId")]
        public Guid RateTypeId { get; set; }

        [JsonProperty("rateTypeName")]
        public string? RateTypeName { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateCourseRateIndexViewModel
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("rateTypeId")]
        public Guid RateTypeId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("calculationType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CalculationType? CalculationType { get; set; }
    }

    public class CreateCourseRateConditionViewModel
    {
        [JsonProperty("academicLevelId")]
        public Guid? AcademicLevelId { get; set; }

        [JsonProperty("facultyId")]
        public Guid? FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("curriculumVersionId")]
        public Guid? CurriculumVersionId { get; set; }

        [JsonProperty("fromBatch")]
        public int? FromBatch { get; set; }

        [JsonProperty("toBatch")]
        public int? ToBatch { get; set; }
    }

    public class CourseRateConditionViewModel : CreateCourseRateConditionViewModel
    {
        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("curriculumVersionName")]
        public string? CurriculumVersionName { get; set; }
    }
}

