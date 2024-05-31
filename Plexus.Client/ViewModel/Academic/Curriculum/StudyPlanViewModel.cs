using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Entity.Utilities;

namespace Plexus.Client.ViewModel.Academic.Curriculum
{
    public class CreateStudyPlanViewModel
    {
        [JsonProperty("curriculumVersionId")]
        public Guid CurriculumVersionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class CreateStudyPlanDetailViewModel
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("term")]
        public string Term { get; set; }

        [JsonProperty("courseIds")]
        public IEnumerable<Guid> CourseIds { get; set; }
    }

    public class UpdateStudyPlanViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class StudyPlanViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("curriculumVersionId")]
        public Guid CurriculumVersionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("years")]
        public IEnumerable<StudyPlanYearViewModel>? Years { get; set; }
    }

    public class StudyPlanYearViewModel
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("terms")]
        public IEnumerable<StudyPlanTermViewModel> Terms { get; set; }
    }

    public class StudyPlanTermViewModel
    {
        [JsonProperty("term")]
        public string Term { get; set; }

        [JsonProperty("courses")]
        public IEnumerable<StudyPlanCourseViewModel>? Courses { get; set; }
    }

    public class StudyPlanCourseViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string? Name => Localizations?.GetDefault().Name;

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

        [JsonProperty("localizations")]
        public IEnumerable<StudyPlanCourseLocalizationViewModel>? Localizations { get; set; }
    }

    public class StudyPlanCourseLocalizationViewModel
    {
        [JsonProperty("language")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(LanguageCode))]
        public LanguageCode Language { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}