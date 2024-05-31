using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic.Advising;

namespace Plexus.Service.ViewModel.Advising
{
    public class AdvisingAppointmentViewModel
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("month")]
        public int Month { get; set; }

        [JsonProperty("slots")]
        public IEnumerable<AdvisingAppointmentSlotViewModel>? Slots { get; set; }
    }

    public class AdvisingAppointmentSlotViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("startedAt")]
        public DateTime StartedAt { get; set; }

        [JsonProperty("endedAt")]
        public DateTime EndedAt { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(AdvisingSlotStatus))]
        [JsonProperty("status")]
        public AdvisingSlotStatus Status { get; set; }
    }
}