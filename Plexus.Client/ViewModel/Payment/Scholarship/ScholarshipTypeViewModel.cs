using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Payment.Scholarship
{
    public class CreateScholarshipTypeViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
    }
    
    public class ScholarshipTypeViewModel : CreateScholarshipTypeViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}