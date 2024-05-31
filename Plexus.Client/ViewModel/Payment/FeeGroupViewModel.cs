using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Payment
{
    public class CreateFeeGroupViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class FeeGroupViewModel : CreateFeeGroupViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}