using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plexus.Database.Model.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Plexus.Entity.DTO.Academic
{
    public class CreateGradeMaintenanceDTO
    {
        public string StudentCode { get; set; }
        public string CoursesCode { get; set; }
        public string Grade { get; set; }
        public string? Remark { get; set; }
        public string? PathFile { get; set; }
        public bool IsActive { get; set; }
    }

    public class GradeMaintenanceDTO : CreateGradeMaintenanceDTO
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }


    public class SearchGradeMaintenanceDTO {

        public string? StudentCode { get; set; }
        public string? Term { get; set; }
        public string? StudentName { get; set; }
        public string? AcademicLevel { get; set; }
        public string? Faculty { get; set; }
        public string? Department { get; set; }
        public string? CurriculumVersion { get; set; }
        public decimal? GPAX { get; set; }
        public string? CompletedCredit { get; set; }
        public string? StudyPlan { get; set; }
        public string? Advisor { get; set; }
}


}
