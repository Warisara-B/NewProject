using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CurriculumInstructorViewModel : CreateCurriculumInstructorViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("instructorRoleName")]
        public string? InstructorRoleName { get; set; }

        [JsonProperty("instructorFirstName")]
        public string? InstructorFirstName { get; set; }

        [JsonProperty("instructorMiddleName")]
        public string? InstructorMiddleName { get; set; }

        [JsonProperty("instructorLastName")]
        public string? InstructorLastName { get; set; }

        [JsonProperty("academicPositionName")]
        public string? AcademicPositionName { get; set; }

        [JsonProperty("careerPositionName")]
        public string? CareerPositionName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateCurriculumInstructorViewModel
    {
        [JsonProperty("instructorId")]
        public Guid InstructorId { get; set; }

        [JsonProperty("instructorRoleId")]
        public Guid InstructorRoleId { get; set; }
    }
}