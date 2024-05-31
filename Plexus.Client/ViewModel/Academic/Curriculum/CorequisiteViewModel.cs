using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CreateCorequisiteViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("corequisiteCourseId")]
        public Guid CorequisiteCourseId { get; set; }
    }

    public class CorequisiteViewModel : CreateCorequisiteViewModel
    {
        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("corequisiteCourseCode")]
        public string? CorequisiteCourseCode { get; set; }

        [JsonProperty("corequisiteCourseName")]
        public string? CorequisiteCourseName { get; set; }
    }
}