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
    public class CourseGradeMaintenViewModel
    {
        [JsonProperty("StudentCode")]
        public string StudentCode { get; set; }

        [JsonProperty("StudentName")]
        public string StudentName { get; set; }
       
        [JsonProperty("AcademicLevelFormalName")]
        public string AcademicLevelFormalName { get; set; }


        [JsonProperty("DepartmentName")]
        public string DepartmentName { get; set; }


        [JsonProperty("CurriculumVersionName")]
        public string CurriculumVersionName { get; set; }

        [JsonProperty("GPAX")]
        public decimal? GPAX { get; set; }

        [JsonProperty("StudyPlans")]
        public string StudyPlans { get; set; }


        [JsonProperty("Course")]
        public string Course { get; set; }

        [JsonProperty("Section")]
        public string Section { get; set; }

        [JsonProperty("Instructor")]
        public string Instructor { get; set; }

        [JsonProperty("Grade")]
        public string Grade { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }
    }

}

