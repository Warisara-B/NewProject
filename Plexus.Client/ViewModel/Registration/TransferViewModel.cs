using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;

namespace Plexus.Client.ViewModel.Registration
{
    public class CreateTransferViewModel
    {
        [JsonProperty("channel")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RegistrationChannel RegistrationChannel { get; set; }

        [JsonProperty("courses")]
        public IEnumerable<CreateTransferCourseViewModel> Courses { get; set; }
    }

    public class TransferViewModel
    {
        [JsonProperty("termId")]
        public Guid TermId { get; set; }

        [JsonProperty("courses")]
        public IEnumerable<TransferCourseViewModel> Courses { get; set; }
    }

    public class TransferCourseViewModel : CreateTransferCourseViewModel
    {

        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string CourseName { get; set; }

        [JsonProperty("credit")]
        public decimal Credit { get; set; }

        [JsonProperty("registrationCredit")]
        public decimal RegistrationCredit { get; set; }

        [JsonProperty("lectureCredit")]
        public decimal LectureCredit { get; set; }

        [JsonProperty("labCredit")]
        public decimal LabCredit { get; set; }

        [JsonProperty("otherCredit")]
        public decimal OtherCredit { get; set; }

        [JsonProperty("gradeLetter")]
        public string? GradeLetter { get; set; }
    }

    public class CreateTransferCourseViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("termId")]
        public Guid TermId { get; set; }

        [JsonProperty("gradeId")]
        public Guid? GradeId { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }
    }
}