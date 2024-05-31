using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Facility
{
    public class CreateRoomTypeViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class RoomTypeViewModel : CreateRoomTypeViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}