using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic;

namespace Plexus.Service.ViewModel
{
    public class AcademicCalendarViewModel
    {
        [JsonProperty("date")]
        public DateOnly Date { get; set; }

        [JsonProperty("events")]
        public IEnumerable<AcademicCalendarEventViewModel>? Events { get; set; }
    }

    public class AcademicCalendarEventViewModel
    {
        [JsonProperty("type")]
        [EnumDataType(typeof(AcademicCalendarEventType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public AcademicCalendarEventType Type { get; set; }

        [JsonProperty("startAt")]
        public DateTime? StartedAt { get; set; }

        [JsonProperty("endAt")]
        public DateTime? EndedAt { get; set; }

        [JsonProperty("cardStartTime")]
        public string? CardStartTime { get; set; }

        [JsonProperty("cardEndTime")]
        public string? CardEndTime { get; set; }

        [JsonProperty("isAllDay")]
        public bool IsAllDay { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("instructors")]
        public IEnumerable<AcademicCalendarInstructorViewModel>? Instructors { get; set; }

        [JsonProperty("class")]
        public AcademicCalendarClassViewModel? Class { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }

        [JsonProperty("isHighlight")]
        public bool IsHighlighted { get; set; }
    }

    public class AcademicCalendarInstructorViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("profileImageUrl")]
        public string? ProfileImageUrl { get; set; }
    }

    public class AcademicCalendarClassViewModel
    {
        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("sectionNumber")]
        public string SectionNumber { get; set; }

        [JsonProperty("credit")]
        public decimal Credit { get; set; }
    }
}