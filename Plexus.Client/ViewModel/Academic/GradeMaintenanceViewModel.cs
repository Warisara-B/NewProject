using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Client.ViewModel.Academic
{
    public class CreateGradeMaintenanceViewModel
    {
        [JsonProperty("StudentCode")]
        public string StudentCode { get; set; }

        [JsonProperty("CoursesCode")]
        public string CoursesCode { get; set; }

        [JsonProperty("Grade")]
        public string Grade { get; set; }

        [JsonProperty("Remark")]
        public string Remark { get; set; }

       
        public IFormFile? File { get; set; }

        [JsonProperty("PathFile")]
        public string? PathFile { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }
    }


    public class GradeMaintenanceViewModel : CreateGradeMaintenanceViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("createdAt")]
        [DefaultValue(typeof(DateTime), "2024-01-01T00:00:00Z")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        [DefaultValue(typeof(DateTime), "2024-01-01T00:00:00Z")]
        public DateTime UpdatedAt { get; set; }

    }

}

