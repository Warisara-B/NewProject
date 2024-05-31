using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Facility.Reservation;

namespace Plexus.Client.ViewModel.Facility.Reservation
{
    public class UpdateRoomReserveSlotViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }
    }

    public class RoomReserveSlotViewModel : UpdateRoomReserveSlotViewModel
    {
        [JsonProperty("roomId")]
        public Guid RoomId { get; set; }

        [JsonProperty("roomName")]
        public string? RoomName { get; set; }

        [JsonProperty("fromDate")]
        public DateTime FromDate { get; set; }

        [JsonProperty("toDate")]
        public DateTime ToDate { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ReservationStatus Status { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}