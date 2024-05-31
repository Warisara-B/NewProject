using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CreateCurriculumCourseViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("requiredGradeId")]
        public Guid? RequiredGradeId { get; set; }
    }

    public class CurriculumCourseViewModel : CreateCurriculumCourseViewModel
    {
        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("requiredGradeLetter")]
        public string? RequiredGradeLetter { get; set; }
    }
}