using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel
{
    public class CreateInstructorRoleViewModel
    {
        [JsonIgnore]
        public string Name => Localizations?.GetDefault().Name;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<InstructorRoleLocalizationViewModel>? Localizations { get; set; }
    }

    public class InstructorRoleViewModel : CreateInstructorRoleViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string? Name { get { return base.Name; } }
    }

    public class InstructorRoleLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        public string? Name { get; set; }
    }

    public class UpdateInstructorRoleSequenceViewModel
    {
        [JsonProperty("sequence")]
        public int Sequence { get; set; }
    }
}