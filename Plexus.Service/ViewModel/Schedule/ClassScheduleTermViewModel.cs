using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.ViewModel.Schedule
{
    public class ClassScheduleTermViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string CourseName { get; set; }

        [JsonProperty("countMember")]
        public int CountMember { get; set; }

        [JsonProperty("credit")]
        public decimal Credit { get; set; }

        [JsonProperty("section")]
        public string Section { get; set; }

        [JsonProperty("location")]
        public string? Location { get; set; }
    }
}
