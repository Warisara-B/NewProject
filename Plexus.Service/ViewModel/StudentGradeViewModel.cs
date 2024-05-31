using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Student;

namespace Plexus.Service.ViewModel
{
    public class StudentGradeViewModel
    {
        [JsonProperty("GPAX")]
        public decimal? GPAX { get; set; }

        [JsonProperty("completedCredit")]
        public decimal? CompletedCredit { get; set; }

        [JsonProperty("terms")]
        public IEnumerable<StudentStudyTermViewModel>? Terms { get; set; }
    }

    public class StudentStudyTermViewModel
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("term")]
        public string? Term { get; set; }

        [JsonProperty("credit")]
        public decimal Credit { get; set; }

        [JsonProperty("gpa")]
        public decimal? GPA { get; set; }

        [JsonProperty("courses")]
        public IEnumerable<StudentCourseViewModel>? Courses { get; set; }
    }

    public class StudentCourseViewModel
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("credit")]
        public decimal? Credit { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        [EnumDataType(typeof(GradeStatus))]
        public GradeStatus Status { get; set; }

        [JsonProperty("grade")]
        public string? Grade { get; set; }

        [JsonProperty("passGrade")]
        public string? PassGrade { get; set; }
    }

    public class StudentCurriculumViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("requiredCredit")]
        public decimal? RequiredCredit { get; set; }

        [JsonProperty("completedCredit")]
        public decimal? CompletedCredit { get; set; }

        [JsonProperty("subGroups")]
        public IEnumerable<StudentCurriculumViewModel>? SubGroups { get; set; }

        [JsonProperty("courses")]
        public IEnumerable<StudentCourseViewModel>? Courses { get; set; }
    }
}