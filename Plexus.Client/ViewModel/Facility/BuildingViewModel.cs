using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Facility
{
    public class CreateBuildingViewModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonIgnore]
        public string? Name => Localizations?.GetDefault().Name;

        [JsonProperty("campusId")]
        public Guid CampusId { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<BuildingLocalizationViewModel>? Localizations { get; set; }
    }

    public class BuildingViewModel : CreateBuildingViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("campusName")]
        public string? CampusName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }
    }

    public class BuildingLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }

    public class BuildingAvailableTimeViewModel
    {
        [JsonProperty("day")]
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek Day { get; set; }

        [JsonProperty("fromTime")]
        public TimeOnly? FromTime { get; set; }

        [JsonProperty("toTime")]
        public TimeOnly? ToTime { get; set; }
    }
}

