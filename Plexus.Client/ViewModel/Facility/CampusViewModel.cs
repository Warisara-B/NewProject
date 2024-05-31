using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Facility
{
    public class CreateCampusViewModel
    {
        [JsonIgnore]
        public string Name => Localizations?.GetDefault().Name;

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonIgnore]
        public string? Address1 => Localizations?.GetDefault().Address1;

        [JsonIgnore]
        public string? Address2 => Localizations?.GetDefault().Address2;

        [JsonProperty("contactNumber")]
        public string? ContactNumber { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<CampusLocalizationViewModel>? Localizations { get; set; }
    }

    public class CampusViewModel : CreateCampusViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }

        [JsonProperty("address1")]
        public new string Address1 { get { return base.Address1; } }

        [JsonProperty("address2")]
        public new string Address2 { get { return base.Address2; } }
    }

    public class CampusLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("address1")]
        public string? Address1 { get; set; }

        [JsonProperty("address2")]
        public string? Address2 { get; set; }
    }
}