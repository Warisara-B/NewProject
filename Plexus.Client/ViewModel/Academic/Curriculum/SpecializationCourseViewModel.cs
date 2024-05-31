using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CreateSpecializationCourseViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("courseCredit")]
        public decimal? Credit { get; set; }

        [JsonProperty("gradeId")]
        public Guid? RequiredGradeId { get; set; }

        [JsonProperty("gradeLetter")]
        public string? RequiredGradeLetter { get; set; }

        [JsonProperty("isRequired")]
        public bool IsRequiredCourse { get; set; }
    }
    public class SpecializationCourseViewModel : CreateSpecializationCourseViewModel
    {

        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }
    }
}