using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Plexus.Entity.DTO.Academic;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateGradingViewModel
    {

        [JsonProperty("studentCode")]
        public string studentCode { get; set; }

        [JsonProperty("studentName")]
        public string studentName { get; set; }

        [JsonProperty("coursesCode")]
        public string coursesCode { get; set; }

        [JsonProperty("section")]
        public string section { get; set; }

        [JsonProperty("mitdtermExam")]
        public string mitdtermExam { get; set; }

        [JsonProperty("mitdtermReport")]
        public string mitdtermReport { get; set; }

        [JsonProperty("finalExam")]
        public string finalExam { get; set; }

        [JsonProperty("totalScore")]
        public string totalScore { get; set; }

        [JsonProperty("mitdterm")]
        public string mitdterm { get; set; }

        [JsonProperty("final")]
        public string final { get; set; }

        [JsonProperty("grade")]
        public string grade { get; set; }

        [JsonProperty("gradingThresholds")]
        public GradingThresholds gradingThresholds { get; set; }
    }


    public class GradingViewModel : CreateGradingViewModel
    {
        [JsonProperty("id")]
        public Guid id { get; set; }

        [JsonProperty("createdAt")]
        public DateTime createdAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime updatedAt { get; set; }
    }
}
