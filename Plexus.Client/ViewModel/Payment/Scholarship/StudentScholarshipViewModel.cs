using System;
using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Payment.Scholarship
{
    public abstract class BaseStudentScholarshipViewModel
    {
        [JsonProperty("startTerm")]
        public int StartTerm { get; set; }

        [JsonProperty("startYear")]
        public int StartYear { get; set; }

        [JsonProperty("endTerm")]
        public int EndTerm { get; set; }

        [JsonProperty("endYear")]
        public int EndYear { get; set; }

        [JsonProperty("isSendContract")]
        public bool IsSendContract { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class CreateStudentScholarshipViewModel : BaseStudentScholarshipViewModel
    {
        [JsonProperty("scholarshipId")]
        public Guid ScholarshipId { get; set; }

        [JsonProperty("startLimitBalance")]
        public decimal StartLimitBalance { get; set; }

        [JsonProperty("reserveBudgets")]
        public IEnumerable<CreateStudentReservedBudgetViewModel> BudgetPools { get; set; }
    }

    public class StudentScholarshipViewModel : CreateStudentScholarshipViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("studentId")]
        public Guid StudentId { get; set; }

        [JsonProperty("studentCode")]
        public string? StudentCode { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }
        
        [JsonProperty("gpa")]
		public decimal? GPA { get; set; }

        [JsonProperty("scholarshipName")]
        public string? ScholarshipName { get; set; }

        [JsonProperty("reserveBudgets")]
        public new IEnumerable<StudentReservedBudgetViewModel> BudgetPools { get; set; }

        [JsonProperty("approvedAt")]
        public DateTime? ApprovedAt { get; set; }

        [JsonProperty("approvedBy")]
        public Guid? ApprovedBy { get; set; }

        [JsonProperty("approvalRemark")]
        public string? ApprovalRemark { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateStudentScholarShipViewModel : BaseStudentScholarshipViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class CreateStudentReservedBudgetViewModel
    {
        [JsonProperty("term")]
        public int? Term { get; set; }

        [JsonProperty("year")]
        public int? Year { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }
    }

    public class StudentReservedBudgetViewModel : CreateStudentReservedBudgetViewModel
    {
        [JsonProperty("id")]
        public Guid? Id { get; set; }

        [JsonProperty("scholarshipId")]
        public Guid ScholarshipId { get; set; }

        [JsonProperty("usedAmount")]
        public decimal UsedAmount { get; set; }

        [JsonProperty("remainingAmount")]
        public decimal RemainingAmount { get; set; }
    }

    public class UpdateStudentReservedBudgetViewModel : CreateStudentReservedBudgetViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }

    public class ApproveStudentScholarshipViewModel
    {
        [JsonProperty("approvedAt")]
        public DateTime ApprovedAt { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }
    }

    public class CreateMultipleScholarshipViewModel
    {
        [JsonProperty("startTerm")]
        public int StartTerm { get; set; }

        [JsonProperty("startYear")]
        public int StartYear { get; set; }
        
        [JsonProperty("studentIds")]
        public IEnumerable<Guid> StudentIds { get; set; }
    }
}

