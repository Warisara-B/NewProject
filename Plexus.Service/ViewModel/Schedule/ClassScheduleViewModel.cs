using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.ViewModel.Schedule
{
    public class ClassScheduleViewModel
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

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string CourseName { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }
}
