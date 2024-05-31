using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.ViewModel.Schedule
{
    public class ClassScheduleDetailViewModel
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

        [JsonProperty("fromDate")]
        public DateTime? FromDate { get; set; }

        [JsonProperty("toDate")]
        public DateTime? ToDate { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string CourseName { get; set; }

        [JsonProperty("courseDescription")]
        public string CourseDescription { get; set; }

        [JsonProperty("credit")]
        public decimal? Credit { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("classLink")]
        public string ClassLink { get; set; }

        [JsonProperty("instructors")]
        public List<InstructorScheduleViewModel> Instructors { get; set; }
    }
}
