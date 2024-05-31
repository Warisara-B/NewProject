using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic.Section
{
    public class CreateExclusionConditionViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("conditions")]
        public IEnumerable<CreateSectionConditionViewModel> Conditions { get; set; }
    }
    
    public class ExclusionConditionViewModel : CreateExclusionConditionViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("sectionId")]
        public Guid SectionId { get; set; }

        [JsonProperty("sectionNumber")]
        public string? SectionNumber { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateSectionConditionViewModel
    {
        [JsonProperty("facultyId")]
        public Guid? FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("curriculumId")]
        public Guid? CurriculumId { get; set; }

        [JsonProperty("curriculumVersionId")]
        public Guid? CurriculumVersionId { get; set; }

        [JsonProperty("batches")]
        public IEnumerable<int>? Batches { get; set; }

        [JsonProperty("codes")]
        public string? Codes { get; set; }
    }

    public class SectionConditionViewModel : CreateSectionConditionViewModel
    {
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