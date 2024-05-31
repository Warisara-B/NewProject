using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Payment.Scholarship
{
    public class ScholarshipReserveBudgetViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }
}