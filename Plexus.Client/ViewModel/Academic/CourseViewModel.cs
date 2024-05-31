using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateCourseViewModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonIgnore]
        public string? Name => Localizations?.GetDefault().Name;

        [JsonIgnore]
        public string? Description => Localizations?.GetDefault().Description;

        [JsonIgnore]
        public string TranscriptName1 => Localizations?.GetDefault().TranscriptName1;

        [JsonIgnore]
        public string? TranscriptName2 => Localizations?.GetDefault().TranscriptName2;

        [JsonIgnore]
        public string? TranscriptName3 => Localizations?.GetDefault().TranscriptName3;

        [JsonProperty("academicLevelId")]
        public Guid AcademicLevelId { get; set; }

        [JsonProperty("facultyId")]
        public Guid? FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("teachingTypeId")]
        public Guid? TeachingTypeId { get; set; }

        [JsonProperty("gradeTemplateId")]
        public Guid? GradeTemplateId { get; set; }

        [JsonProperty("credit")]
        public decimal Credit { get; set; }

        [JsonProperty("registrationCredit")]
        public decimal RegistrationCredit { get; set; }

        [JsonProperty("paymentCredit")]
        public decimal PaymentCredit { get; set; }

        [JsonProperty("hour")]
        public decimal Hour { get; set; }

        [JsonProperty("lectureCredit")]
        public decimal LectureCredit { get; set; }

        [JsonProperty("labCredit")]
        public decimal LabCredit { get; set; }

        [JsonProperty("otherCredit")]
        public decimal OtherCredit { get; set; }

        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("localizations")]
        public IEnumerable<CourseLocalizationViewModel>? Localizations { get; set; }
    }

    public class CourseViewModel : CreateCourseViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("academicLevelName")]
        public string? AcademicLevelName { get; set; }

        [JsonProperty("facultyName")]
        public string? FacultyName { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }

        [JsonProperty("teachingTypeName")]
        public string? TeachingTypeName { get; set; }

        [JsonProperty("gradeTemplateName")]
        public string? GradeTemplateName { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("name")]
        public new string Name { get { return base.Name; } }

        [JsonProperty("description")]
        public new string Description { get { return base.Description; } }

        [JsonProperty("transcriptName1")]
        public new string TranscriptName1 { get { return base.TranscriptName1; } }

        [JsonProperty("transcriptName2")]
        public new string TranscriptName2 { get { return base.TranscriptName2; } }

        [JsonProperty("transcriptName3")]
        public new string TranscriptName3 { get { return base.TranscriptName3; } }

    }

    public class CourseLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("transcriptName1")]
        public string? TranscriptName1 { get; set; }

        [JsonProperty("transcriptName2")]
        public string? TranscriptName2 { get; set; }

        [JsonProperty("transcriptName3")]
        public string? TranscriptName3 { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }
    }
}

