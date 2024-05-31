using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Client.ViewModel.Academic.Section;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateStudyCourseViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("sectionId")]
        public Guid? SectionId { get; set; }

        [JsonProperty("gradeId")]
        public Guid? GradeId { get; set; }

        [JsonProperty("gradePublishedAt")]
        public DateTime? GradePublishedAt { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StudyCourseStatus Status { get; set; }

        [JsonProperty("channel")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RegistrationChannel RegistrationChannel { get; set; }
    }

    public class StudyCourseViewModel : CreateStudyCourseViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("termId")]
        public Guid TermId { get; set; }

        [JsonProperty("studentId")]
        public Guid StudentId { get; set; }

        [JsonProperty("studentCode")]
        public string StudentCode { get; set; }

        [JsonProperty("firstName")]
        public string? FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string? LastName { get; set; }

        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string CourseName { get; set; }

        [JsonProperty("credit")]
        public decimal Credit { get; set; }

        [JsonProperty("registrationCredit")]
        public decimal RegistrationCredit { get; set; }

        [JsonProperty("hour")]
        public decimal Hour { get; set; }

        [JsonProperty("lectureCredit")]
        public decimal LectureCredit { get; set; }

        [JsonProperty("labCredit")]
        public decimal LabCredit { get; set; }

        [JsonProperty("otherCredit")]
        public decimal OtherCredit { get; set; }

        [JsonProperty("sectionNumber")]
        public string? SectionNumber { get; set; }

        [JsonProperty("mainInstructorCode")]
        public string? MainInstructorCode { get; set; }

        [JsonProperty("mainInstructorFirstName")]
        public string? MainInstructorFirstName { get; set; }

        [JsonProperty("mainInstructorMiddleName")]
        public string? MainInstructorMiddleName { get; set; }

        [JsonProperty("mainInstructorLastName")]
        public string? MainInstructorLastName { get; set; }

        [JsonProperty("paidAt")]
        public DateTime? PaidAt { get; set; }

        [JsonProperty("gradeLetter")]
        public string? GradeLetter { get; set; }

        [JsonProperty("gradeWeight")]
        public decimal? GradeWeight { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("details")]
        public IEnumerable<SectionDetailViewModel>? Details { get; set; }
    }

    public class UpdateStudyCourseViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("gradeId")]
        public Guid? GradeId { get; set; }

        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public StudyCourseStatus Status { get; set; }

        [JsonProperty("gradePublishedAt")]
        public DateTime? GradePublishedAt { get; set; }
    }
}

