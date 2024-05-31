using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic;
using Plexus.Entity.Utilities;
using Plexus.Utility.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Plexus.Entity.DTO
{
    public class SearchGradeMaintenanceViewModel
    {
        [FromQuery(Name = "StudentsCode")]
        public string? StudentCode { get; set; }

        [FromQuery(Name = "Term")]
        public string? Term { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }


    public class CourseGradeMaintenanceViewModel
    {
        [FromQuery(Name = "Course")]
        public string? Course { get; set; }

        [FromQuery(Name = "Section")]
        public string? Section { get; set; }

        [FromQuery(Name = "Instructor")]
        public string? Instructor { get; set; }

        [FromQuery(Name = "Grade")]
        public string? Grade { get; set; }

        [FromQuery(Name = "Status")]
        public string? Status { get; set; }
    }

    public class SearchProfileGradeMaintenanceViewModel
    {
        [FromQuery(Name = "StudentsCode")]
        public string? StudentCode { get; set; }

        [FromQuery(Name = "Term")]
        public string? Term { get; set; }

        [FromQuery(Name = "StudentName")]
        public string? StudentName { get; set; }

        [FromQuery(Name = "AcademicLevel")]
        public string? AcademicLevel { get; set; }

        [FromQuery(Name = "Faculty")]
        public string? Faculty { get; set; }

        [FromQuery(Name = "Department")]
        public string? Department { get; set; }

        [FromQuery(Name = "CurriculumVersion")]
        public string? CurriculumVersion { get; set; }

        [FromQuery(Name = "GPAX")]
        public decimal? GPAX { get; set; }

        [FromQuery(Name = "CompletedCredit")]
        public string? CompletedCredit { get; set; }

        [FromQuery(Name = "StudyPlan")]
        public string? StudyPlan { get; set; }

        [FromQuery(Name = "Advisor")]
        public string? Advisor { get; set; }

        [FromQuery(Name = "CoursehGradeMainor")]
        public List<CourseGradeMaintenanceViewModel> CoursehGradeMainor { get; set; }

    }

}

