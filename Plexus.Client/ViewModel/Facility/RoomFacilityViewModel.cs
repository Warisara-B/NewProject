using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Facility
{
    public class UpdateRoomFacilityViewModel
    {
        [JsonProperty("facilityId")]
        public Guid FacilityId { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class RoomFacilityViewModel : UpdateRoomFacilityViewModel
    {
        [JsonProperty("facilityName")]
        public string? FacilityName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }
    }
}