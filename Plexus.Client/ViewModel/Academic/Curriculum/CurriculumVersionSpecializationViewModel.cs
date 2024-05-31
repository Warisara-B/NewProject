using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CurriculumVersionSpecializationViewModel
    {
        [JsonProperty("academicSpecializationId")]
		public Guid AcademicSpecializationId { get; set; }

        [JsonProperty("academicSpecializationCode")]
        public string? AcademicSpecializationCode { get; set; }

        [JsonProperty("academicSpecializationName")]
        public string? AcademicSpecializationName { get; set; }
    }
}