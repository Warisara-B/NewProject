using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CopyCurriculumVersionViewModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonIgnore]
        public string? Name => Localizations?.GetDefault().Name;

        [JsonIgnore]
        public string? DegreeName => Localizations?.GetDefault().DegreeName;

        [JsonIgnore]
        public string? Description => Localizations?.GetDefault().Description;

        [JsonIgnore]
        public string? Abbreviation => Localizations?.GetDefault().Abbreviation;

        [JsonProperty("totalCredit")]
        public decimal TotalCredit { get; set; }

        [JsonProperty("totalYear")]
        public decimal TotalYear { get; set; }

        [JsonProperty("expectedGraduatingCredit")]
        public decimal ExpectedGraduatingCredit { get; set; }

        [JsonProperty("approvedAt")]
        public DateTime ApprovedAt { get; set; }

        [JsonProperty("startBatchCode")]
        public int StartBatchCode { get; set; }

        [JsonProperty("endBatchCode")]
        public int EndBatchCode { get; set; }

        [JsonProperty("collegeCalendarType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CollegeCalendarType CollegeCalendarType { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; } = string.Empty;

        [JsonProperty("localizations")]
        public IEnumerable<CurriculumVersionLocalizationViewModel>? Localizations { get; set; }
    }

    public class CreateCurriculumVersionViewModel : CopyCurriculumVersionViewModel
    {
        [JsonProperty("curriculumId")]
        public Guid? CurriculumId { get; set; }

        [JsonProperty("academicLevelId")]
        public Guid AcademicLevelId { get; set; }

        [JsonProperty("facultyId")]
        public Guid FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid DepartmentId { get; set; }

        [JsonProperty("academicProgramId")]
        public Guid AcademicProgramId { get; set; }
    }

    public class CurriculumVersionViewModel : CreateCurriculumVersionViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("curriculumName")]
        public string? CurriculumName { get; set; }

        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("academicProgramName")]
        public string? AcademicProgramName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string? Name { get { return base.Name; } }

        [JsonProperty("degreeName")]
        public new string? DegreeName { get { return base.DegreeName; } }

        [JsonProperty("description")]
        public new string? Description { get { return base.Description; } }

        [JsonProperty("abbreviation")]
        public new string? Abbreviation { get { return base.Abbreviation; } }
    }

    public class CurriculumVersionLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("degreeName")]
        public string? DegreeName { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("abbreviation")]
        public string? Abbreviation { get; set; }
    }
}