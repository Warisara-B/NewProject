using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Facility.Reservation;

namespace Plexus.Client.ViewModel.Facility.Reservation
{
    public class CreateRoomReserveRequestViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("senderType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SenderType SenderType { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("fromDate")]
        public DateTime FromDate { get; set; }

        [JsonProperty("toDate")]
        public DateTime? ToDate { get; set; }

        [JsonProperty("startedAt")]
        public TimeSpan StartedAt { get; set; }

        [JsonProperty("endedAt")]
		public TimeSpan EndedAt { get; set; }
        
        [JsonProperty("repeatedOn", ItemConverterType=typeof(StringEnumConverter))]
		public IEnumerable<DayOfWeek>? RepeatedOn { get; set; }

        [JsonProperty("roomId")]
        public Guid RoomId { get; set; }

        [JsonProperty("usageType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public UsageType UsageType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("requesterName")]
        public string RequesterName { get; set; }
    }

    public class RoomReserveRequestViewModel : CreateRoomReserveRequestViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReservationStatus Status { get; set; }
        
        [JsonProperty("slots")]
        public IEnumerable<RoomReserveSlotViewModel> Slots { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
		public DateTime UpdatedAt { get; set; }

        [JsonProperty("roomName")]
        public string? RoomName { get; set; }
    }
}