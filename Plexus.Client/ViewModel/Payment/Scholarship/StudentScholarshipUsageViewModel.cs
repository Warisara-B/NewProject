using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Payment.Scholarship
{
    public class StudentScholarshipUsageViewModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("scholarshipId")]
        public Guid ScholarshipId { get; set; }
        
        [JsonProperty("scholarshipName")]
        public string? ScholarshipName { get; set; }

        [JsonProperty("year")]
        public int? Year { get; set; }

        [JsonProperty("term")]
        public int? Term { get; set; }

        [JsonProperty("documentNumber")]
        public string? DocumentNumber { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}