using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Registration
{
    public class RegistrationLogCourseViewModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("lectureCredit")]
        public decimal LectureCredit { get; set; }

        [JsonProperty("labCredit")]
        public decimal LabCredit { get; set; }

        [JsonProperty("otherCredit")]
        public decimal OtherCredit { get; set; }

        [JsonProperty("sectionNumber")]
        public string? SectionNumber { get; set; }
    }
}