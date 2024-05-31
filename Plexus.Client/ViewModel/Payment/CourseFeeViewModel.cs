using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Payment;

namespace Plexus.Client.ViewModel.Payment
{
    public class CreateCourseFeeViewModel
    {
        [JsonProperty("feeItemId")]
        public Guid FeeItemId { get; set; }

        [JsonProperty("calculationType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CalculationType CalculationType { get; set; }

        [JsonProperty("condition")]
        public UpdateCourseFeeConditionViewModel? Condition { get; set; }

        [JsonProperty("rateTypeId")]
        public Guid RateTypeId { get; set; }

        [JsonProperty("rateIndex")]
        public int? RateIndex { get; set; }
    }

    public class CourseFeeViewModel : CreateCourseFeeViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("feeItemName")]
        public string? FeeItemName { get; set; }

        [JsonProperty("rateTypeName")]
        public string? RateTypeName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateCourseFeeConditionViewModel
    {
        [JsonProperty("sectionNumber")]
        public string? SectionNumber { get; set; }

        [JsonProperty("academicLevelId")]
        public Guid? AcademicLevelId { get; set; }

        [JsonProperty("fromBatch")]
        public int? FromBatch { get; set; }

        [JsonProperty("toBatch")]
        public int? ToBatch { get; set; }

        [JsonProperty("facultyId")]
        public Guid? FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("curriculumId")]
        public Guid? CurriculumId { get; set; }

        [JsonProperty("curriculumVersionId")]
        public Guid? CurriculumVersionId { get; set; }

        [JsonProperty("studentFeeTypeId")]
        public Guid? StudentFeeTypeId { get; set; }
    }

    public class CourseFeeConditionViewModel : UpdateCourseFeeConditionViewModel
    {
        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("curriculumName")]
        public string? CurriculumName { get; set; }

        [JsonProperty("curriculumVersionName")]
        public string? CurriculumVersionName { get; set; }

        [JsonProperty("studentFeeTypeName")]
        public string? StudentFeeTypeName { get; set; }
    }
}