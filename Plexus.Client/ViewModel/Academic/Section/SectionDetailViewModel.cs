using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic.Section;

namespace Plexus.Client.ViewModel.Academic.Section
{
    public class UpdateSectionDetailViewModel
    {
        [JsonProperty("day")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek Day { get; set; }

        [JsonProperty("startTime")]
        public TimeSpan StartTime { get; set; }

        [JsonProperty("endTime")]
        public TimeSpan EndTime { get; set; }

        [JsonProperty("roomId")]
        public Guid? RoomId { get; set; }

        [JsonProperty("instructorId")]
        public Guid? InstructorId { get; set; }

        [JsonProperty("teachingTypeId")]
        public Guid? TeachingTypeId { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }
    }

    public class SectionDetailViewModel : UpdateSectionDetailViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("roomName")]
        public string? RoomName { get; set; }

        [JsonProperty("instructorCode")]
        public string? InstructorCode { get; set; }

        [JsonProperty("instructorFirstName")]
        public string? InstructorFirstName { get; set; }

        [JsonProperty("instructorMiddleName")]
        public string? InstructorMiddleName { get; set; }

        [JsonProperty("instructorLastName")]
        public string? InstructorLastName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}

