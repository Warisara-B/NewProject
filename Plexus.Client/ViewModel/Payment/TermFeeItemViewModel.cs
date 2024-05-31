using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Payment;

namespace Plexus.Client.ViewModel.Payment
{
    public class CreateTermFeeItemViewModel
    {
        [JsonProperty("feeItemId")]
        public Guid FeeItemId { get; set; }

        [JsonProperty("recurringType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RecurringType RecurringType { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("termNo")]
        public string? TermNo { get; set; }

        [JsonProperty("termRunning")]
        public int? TermRunning { get; set; }

        [JsonProperty("termType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TermType TermType { get; set; }

        [JsonProperty("condition")]
        public IEnumerable<CreateTermFeeItemConditionViewModel>? Condition { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class TermFeeItemViewModel : CreateTermFeeItemViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("termFeePackageId")]
        public Guid TermFeePackageId { get; set; }

        [JsonProperty("termFeePackageName")]
        public string? TermFeePackageName { get; set; }

        [JsonProperty("feeItemName")]
        public string? FeeItemName { get; set; }

        [JsonProperty("condition")]
        public new TermFeeItemConditionViewModel? Condition { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateTermFeeItemConditionViewModel
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

    public class TermFeeItemConditionViewModel : CreateTermFeeItemConditionViewModel
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
    }
}