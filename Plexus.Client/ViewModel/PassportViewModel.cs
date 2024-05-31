using Newtonsoft.Json;

namespace Plexus.Client.ViewModel
{
    public class CreatePassportViewModel
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("issuedAt")]
        public DateTime IssuedAt { get; set; }

        [JsonProperty("expiredAt")]
        public DateTime ExpiredAt { get; set; }

        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }

    public class PassportViewModel : CreatePassportViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("studentId")]
        public Guid StudentId { get; set; }
    }
}