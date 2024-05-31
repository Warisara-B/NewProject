using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;

namespace Plexus.Client.ViewModel.Registration
{
    public class RegistrationLogViewModel
    {
        [JsonProperty("channel")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RegistrationChannel RegistrationChannel { get; set; }

        [JsonProperty("newCourses")]
        public IEnumerable<RegistrationLogCourseViewModel> NewCourses { get; set; }

        [JsonProperty("dropCourses")]
        public IEnumerable<RegistrationLogCourseViewModel> DropCourses { get; set; }

        [JsonProperty("summary")]
        public IEnumerable<RegistrationLogCourseViewModel> Summary { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
            
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }
    }
}