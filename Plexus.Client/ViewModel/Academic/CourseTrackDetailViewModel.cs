using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic
{
    public class UpdateCourseTrackDetailViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("step")]
        public int Step { get; set; }

        [JsonProperty("isRequired")]
        public bool IsRequired { get; set; }
    }

    public class CourseTrackDetailViewModel : UpdateCourseTrackDetailViewModel
    {
        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("courseTrackId")]
        public Guid CourseTrackId { get; set; }
    }
}