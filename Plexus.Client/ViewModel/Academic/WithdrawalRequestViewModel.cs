using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateWithdrawalRequestViewModel
    {
        [JsonProperty("sectionIds")]
        public IEnumerable<Guid> SectionIds { get; set; }

        [JsonProperty("studentIds")]
        public IEnumerable<Guid> StudentIds { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }
    }

    public class WithdrawalRequestViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("studentId")]
        public Guid StudentId { get; set; }

        [JsonProperty("student")]
        public StudentViewModel Student { get; set; }

        [JsonProperty("sectionId")]
        public Guid? SectionId { get; set; }

        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string CourseName { get; set; }

        [JsonProperty("sectionNumber")]
        public string? SectionNumber { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WithdrawalStatus Status { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("approvedAt")]
        public DateTime? ApprovedAt { get; set; }
    }

    public class UpdateWithdrawalStatusViewModel
    {
        [JsonProperty("ids")]
        public IEnumerable<Guid> Ids { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WithdrawalStatus Status { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }
    }
}

