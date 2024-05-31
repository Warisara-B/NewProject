using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Registration
{
    public class CoursePrerequisiteViewModel : BasePrerequisiteViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("curriculumVersions")]
        public IEnumerable<CurriculumCoursePrerequisiteViewModel> CurriculumVersions { get; set; }
    }
}