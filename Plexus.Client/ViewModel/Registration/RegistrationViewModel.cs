using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;

namespace Plexus.Client.ViewModel.Registration
{
     public class RegistrationViewModel
     {
          [JsonProperty("studentIds")]
          public IEnumerable<Guid> StudentIds { get; set; }

          [JsonProperty("termId")]
          public Guid TermId { get; set; }

          [JsonProperty("channel")]
          [JsonConverter(typeof(StringEnumConverter))]
          public RegistrationChannel RegistrationChannel { get; set; }

          [JsonProperty("sections")]
          public IEnumerable<RegistrationCourseViewModel>? Sections { get; set; }
     }

     public class RegistrationCourseViewModel
     {
          [JsonProperty("courseId")]
          public Guid CourseId { get; set; }

          [JsonProperty("sectionId")]
          public Guid? SectionId { get; set; }
     }
}