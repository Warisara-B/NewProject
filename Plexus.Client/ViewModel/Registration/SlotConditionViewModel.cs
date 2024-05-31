using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Registration
{
    public class CreateSlotConditionViewModel
    {
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("conditions")]
        public IEnumerable<CreateConditionViewModel> Conditions { get; set; }
    }

    public class SlotConditionViewModel : CreateSlotConditionViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("slotId")]
        public Guid SlotId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("conditions")]
        public new IEnumerable<ConditionViewModel> Conditions { get; set; }
    }

    public class CreateConditionViewModel
    {
        [JsonProperty("facultyId")]
        public Guid? FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("codes")]
        public string? Codes { get; set; }

        [JsonProperty("startedCode")]
        public string? StartedCode { get; set; }

        [JsonProperty("endedCode")]
        public string? EndedCode { get; set; }

        [JsonProperty("startedBatch")]
        public int? StartedBatch { get; set; }

        [JsonProperty("endedBatch")]
        public int? EndedBatch { get; set; }
    }

    public class ConditionViewModel : CreateConditionViewModel
    {
        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }
    }
}