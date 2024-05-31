using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Payment.Scholarship
{
    public class CreateScholarshipViewModel
    {
        [JsonProperty("scholarshipTypeId")]
        public Guid ScholarshipTypeId { get; set; }

        [JsonProperty("sponsor")]
        public string Sponsor { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("totalBudget")]
        public decimal TotalBudget { get; set; }

        [JsonProperty("limitBalance")]
        public decimal LimitBalance { get; set; }

        [JsonProperty("yearDuration")]
        public int YearDuration { get; set; }

        [JsonProperty("minGPA")]
        public decimal? MinGPA { get; set; }

        [JsonProperty("maxGPA")]
        public decimal? MaxGPA { get; set; }

        [JsonProperty("isRepeatCourseApplied")]
        public bool IsRepeatCourseApplied { get; set; }

        [JsonProperty("isAllCoverage")]
        public bool IsAllCoverage { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("budgets")]
        public IEnumerable<ScholarshipReserveBudgetViewModel>? Budgets { get; set; }

        [JsonProperty("feeItems")]
        public IEnumerable<UpdateScholarshipFeeItemViewModel>? FeeItems { get; set; }
    }

    public class ScholarshipViewModel : CreateScholarshipViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("totalStudents")]
        public int TotalStudents { get; set; }

        [JsonProperty("feeItems")]
        public new IEnumerable<ScholarshipFeeItemViewModel>? FeeItems { get; set; }

        [JsonProperty("scholarshipTypeName")]
        public string? ScholarshipTypeName { get; set; }
        
        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}