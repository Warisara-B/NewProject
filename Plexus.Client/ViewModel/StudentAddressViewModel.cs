using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;

namespace Plexus.Client.ViewModel
{
    public class CreateStudentAddressViewModel
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AddressType Type { get; set; }

        [JsonProperty("address1")]
        public string? Address1 { get; set; }

        [JsonProperty("address2")]
        public string? Address2 { get; set; }

        [JsonProperty("houseNumber")]
        public string? HouseNumber { get; set; }

        [JsonProperty("moo")]
        public string? Moo { get; set; }

        [JsonProperty("soi")]
        public string? Soi { get; set; }

        [JsonProperty("road")]
        public string? Road { get; set; }

        [JsonProperty("province")]
        public string? Province { get; set; }

        [JsonProperty("district")]
        public string? District { get; set; }

        [JsonProperty("subDistrict")]
        public string? SubDistrict { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("postalCode")]
        public string? PostalCode { get; set; }
    }

    public class StudentAddressViewModel : CreateStudentAddressViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("studentId")]
        public Guid StudentId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}