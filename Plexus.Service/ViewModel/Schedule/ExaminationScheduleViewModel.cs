using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.ViewModel.Schedule
{
    public class ExaminationScheduleViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("startAt")]
        public DateTime? StartAt { get; set; }

        [JsonProperty("endAt")]
        public DateTime? EndAt { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }

        [JsonProperty("examType")]
        [EnumDataType(typeof(ExamType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public ExamType? ExamType { get; set; }

        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string CourseName { get; set; }

        [JsonProperty("credit")]
        public decimal Credit { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }

        [JsonProperty("proctor")]
        public List<ScheduleProctorViewModel> Proctor { get; set; }
    }

    public class ScheduleProctorViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("profileImageUrl")]
        public string ProfileImageUrl { get; set; }
    }
}
