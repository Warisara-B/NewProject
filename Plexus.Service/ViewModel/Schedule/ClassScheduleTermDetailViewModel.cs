using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.ViewModel.Schedule
{
    public class ClassScheduleTermDetailViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("fromDate")]
        public DateTime? FromDate { get; set; }

        [JsonProperty("toDate")]
        public DateTime? ToDate { get; set; }

        [JsonProperty("timeDetail")]
        public List<TimeDetail> TimeDetail { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string CourseName { get; set; }

        [JsonProperty("courseDescription")]
        public string? CourseDescription { get; set; }

        [JsonProperty("credit")]
        public decimal Credit { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }

        [JsonProperty("classLink")]
        public string? ClassLink { get; set; }

        [JsonProperty("instructors")]
        public List<InstructorScheduleViewModel> Instructors { get; set; }
    }

    public class TimeDetail
    {
        [JsonProperty("dayOfWeek")]
        [EnumDataType(typeof(DayOfWeek))]
        [JsonConverter(typeof(StringEnumConverter))]
        public DayOfWeek DayOfWeek { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("endTime")]
        public string EndTime { get; set; }
    }
}
