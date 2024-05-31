using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic;

namespace Plexus.Client.ViewModel
{
    public class TermStatusCheckViewModel
    {
        [JsonProperty("academicLevelId")]
        public Guid AcademicLevelId { get; set; }

        [JsonProperty("termId")]
        public Guid? TermId { get; set; }

        [JsonProperty("calendarType")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(CollegeCalendarType))]
        public CollegeCalendarType CollegeCalendarType { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(TermStatus))]
        public TermStatus Status { get; set; }
    }
}