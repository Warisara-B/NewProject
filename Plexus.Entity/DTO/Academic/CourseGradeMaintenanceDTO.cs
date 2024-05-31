using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Entity.DTO.Academic
{
    public class ProfileGradeMaintenanceDTO
    {
        public string? StudentName { get; set; }
        public string? AcademicLevelFormalName { get; set; }
        public string? DepartmentName { get; set; }
        public string? CurriculumVersionName { get; set; }
        public decimal? GPAX { get; set; }
        public string? StudyPlans { get; set; }
    }

    public class CourseGradeMaintenanceDTO : ProfileGradeMaintenanceDTO
    {
        public string? StudentCode { get; set; }
        public string? Term { get; set; }
        public string? Course { get; set; }
        public string? Section { get; set; }
        public string? Instructor { get; set; }
        public string? Grade { get; set; }
        public string? Status { get; set; }
    }

}
