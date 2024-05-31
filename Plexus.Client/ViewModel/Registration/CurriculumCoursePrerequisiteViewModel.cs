using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Registration
{
    public class CurriculumCoursePrerequisiteViewModel : CoursePrerequisiteViewModel
    {
        [JsonProperty("curriculumVersionId")]
        public Guid CurriculumVersionId { get; set; }

        [JsonProperty("curriculumVersionName")]
        public string? CurriculumVersionName { get; set; }
    }
}