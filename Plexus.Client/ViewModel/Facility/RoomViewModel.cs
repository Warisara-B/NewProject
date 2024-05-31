using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Facility
{
    public class CreateRoomViewModel
    {
        [JsonProperty("buildingId")]
        public Guid? BuildingId { get; set; }

        [JsonIgnore]
        public string? Name => Localizations?.GetDefault().Name;

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("floor")]
        public int? Floor { get; set; }

        [JsonProperty("capacity")]
        public int? Capacity { get; set; }

        [JsonProperty("examCapacity")]
        public int? ExaminationCapacity { get; set; }

        [JsonProperty("roomTypeId")]
        public Guid? RoomTypeId { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("isReservable")]
        public bool IsReservable { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<RoomLocalizationViewModel>? Localizations { get; set; }

        [JsonProperty("facilities")]
        public IEnumerable<UpdateRoomFacilityViewModel>? Facilities { get; set; }
    }

    public class RoomViewModel : CreateRoomViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("buildingName")]
        public string? BuildingName { get; set; }

        [JsonProperty("roomTypeName")]
        public string? RoomTypeName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("facilities")]
        public new IEnumerable<RoomFacilityViewModel>? Facilities { get; set; }

        [JsonProperty("name")]
        public new string? Name { get { return base.Name; } }
    }

    public class RoomLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}