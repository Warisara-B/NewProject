using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Entity.DTO.Academic.Section;

namespace Plexus.Client.ViewModel.Academic.Section
{
    public class CreateSectionSeatViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SeatType Type { get; set; }

        [JsonProperty("totalSeat")]
        public int TotalSeat { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }

        [JsonProperty("conditions")]
        public IEnumerable<CreateSectionConditionViewModel> Conditions { get; set; }
    }

    public class SectionSeatViewModel : UpsertSectionSeatViewModel
    {
        [JsonProperty("id")]
        public new Guid Id { get; set; }

        [JsonProperty("sectionId")]
        public Guid SectionId { get; set; }

        [JsonProperty("masterSeatId")]
        public Guid? MasterSeatId { get; set; }

        [JsonProperty("seatUsed")]
        public int SeatUsed { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("conditions")]
        public new IEnumerable<SectionConditionViewModel> Conditions { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }

    public class UpsertSectionSeatViewModel : CreateSectionSeatViewModel
    {
        [JsonProperty("id")]
        public Guid? Id { get; set; }
    }
}

