using Newtonsoft.Json;

namespace Plexus.Client.ViewModel
{
    public class CreateDeformationViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("bookCode")]
        public string BookCode { get; set; }

        [JsonProperty("issuedAt")]
        public DateTime IssuedAt { get; set; }

        [JsonProperty("expiredAt")]
        public DateTime ExpiredAt { get; set; }
    }

    public class DeformationViewModel : CreateDeformationViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("studentId")]
        public Guid StudentId { get; set; }
    }
}