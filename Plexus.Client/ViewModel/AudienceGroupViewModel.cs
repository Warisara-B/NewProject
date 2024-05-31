using Newtonsoft.Json;

namespace Plexus.Client.ViewModel
{
    public class CreateAudienceGroupViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("conditions")]
        public IEnumerable<CreateAudienceGroupConditionViewModel> Conditions { get; set; }
    }

    public class AudienceGroupViewModel : CreateAudienceGroupViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("conditions")]
        public new IEnumerable<AudienceGroupConditionViewModel> Conditions { get; set; }
    }

    public class CreateAudienceGroupConditionViewModel
    {
        [JsonProperty("academicLevelId")]
        public Guid? AcademicLevelId { get; set; }

        [JsonProperty("academicProgramId")]
        public Guid? AcademicProgramId { get; set; }
        
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

        [JsonProperty("startedLastDigit")]
        public string? StartedLastDigit { get; set; }
        
        [JsonProperty("endedLastDigit")]
        public string? EndedLastDigit { get; set; }
    }

    public class AudienceGroupConditionViewModel : CreateAudienceGroupConditionViewModel
    {
        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("academicProgramName")]
        public string? AcademicProgramName { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }
    }
}