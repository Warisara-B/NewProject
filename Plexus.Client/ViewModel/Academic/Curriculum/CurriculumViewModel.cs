using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
	public class CreateCurriculumViewModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("academicLevelId")]
		public Guid AcademicLevelId { get; set; }
        
        [JsonProperty("facultyId")]
		public Guid FacultyId { get; set; }

		[JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

		[JsonProperty("name")]
        public string Name { get; set; }
		
		[JsonProperty("formalName")]
        public string? FormalName { get; set; }
		
		[JsonProperty("abbreviation")]
        public string? Abbreviation { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; } = string.Empty;

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<CurriculumLocalizationViewModel>? Localizations { get; set; }
    }

	public class CurriculumViewModel : CreateCurriculumViewModel
	{
        [JsonProperty("id")]
		public Guid Id { get; set; }

        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
	}

    public class CurriculumLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("formalName")]
        public string? FormalName { get; set; }

        [JsonProperty("abbreviation")]
        public string? Abbreviation { get; set; }
    }
}

