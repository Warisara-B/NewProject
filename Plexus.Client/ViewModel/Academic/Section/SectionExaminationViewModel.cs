using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic;

namespace Plexus.Client.ViewModel.Academic.Section
{
    public class UpdateSectionExaminationViewModel
    {
        [JsonProperty("examType")]
		[JsonConverter(typeof(StringEnumConverter))]
        public ExamType ExamType { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("startTime")]
        public TimeSpan StartTime { get; set; }

        [JsonProperty("endTime")]
		public TimeSpan EndTime { get; set; }

        [JsonProperty("roomId")]
        public Guid? RoomId { get; set; }
    }

    public class SectionExaminationViewModel : UpdateSectionExaminationViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("sectionId")]
        public Guid SectionId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("roomName")]
        public string? RoomName { get; set; }
    }
}