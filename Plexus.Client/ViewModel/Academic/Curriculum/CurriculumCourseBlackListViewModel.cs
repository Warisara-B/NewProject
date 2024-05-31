using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CurriculumCourseBlackListViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("courseCredit")]
        public decimal? Credit { get; set; }
    }
}

