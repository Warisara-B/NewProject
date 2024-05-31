using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Payment.Scholarship
{
    public class UpdateScholarshipFeeItemViewModel
    {
        [JsonProperty("feeItemId")]
        public Guid FeeItemId { get; set; }

        [JsonProperty("percentage")]
        public decimal? Percentage { get; set; }

        [JsonProperty("amount")]
        public decimal? Amount { get; set; }
    }

    public class ScholarshipFeeItemViewModel : UpdateScholarshipFeeItemViewModel
    {
        [JsonProperty("feeItemName")]
        public string? FeeItemName { get; set; }
    }
}