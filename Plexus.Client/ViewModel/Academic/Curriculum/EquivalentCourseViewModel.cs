using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CreateEquivalentCourseViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("equivalentCourseId")]
        public Guid EquivalentCourseId { get; set; }
    }

    public class EquivalentCourseViewModel : CreateEquivalentCourseViewModel
    {
        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("equivalentCourseCode")]
        public string? EquivalentCourseCode { get; set; }

        [JsonProperty("equivalentCourseName")]
        public string? EquivalentCourseName { get; set; }
    }
}