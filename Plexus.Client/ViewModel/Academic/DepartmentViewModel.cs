using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateDepartmentViewModel
    {
        [FromForm(Name = "facultyId")]
        public Guid FacultyId { get; set; }

        [FromForm(Name = "code")]
        public string Code { get; set; }

        [JsonIgnore]
        public string Name => Localizations?.GetDefault().Name;

        [JsonIgnore]
        public string? FormalName => Localizations?.GetDefault().FormalName;

        [FromForm(Name = "logoImage")]
        public IFormFile? LogoImage { get; set; }

        [FromForm(Name = "isActive")]
        public bool IsActive { get; set; }

        [FromForm(Name = "localizations")]
        public IEnumerable<DepartmentLocalizationViewModel>? Localizations { get; set; }
    }

    public class DepartmentViewModel : CreateDepartmentViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("logoImageURL")]
        public string? LogoImageURL { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }

        [JsonProperty("formalName")]
        public new string FormalName { get { return base.FormalName; } }
    }

    public class DepartmentLocalizationViewModel
    {
        [FromForm(Name = "language")]
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [FromForm(Name = "name")]
        public string? Name { get; set; }

        [FromForm(Name = "formalName")]
        public string? FormalName { get; set; }
    }

    public class UpdateDepartmentViewModel : CreateDepartmentViewModel
    {
        [JsonProperty("deleteLogo")]
        public bool DeleteLogo { get; set; }
    }
}