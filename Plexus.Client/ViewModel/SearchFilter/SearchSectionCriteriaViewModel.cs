using Microsoft.AspNetCore.Mvc;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.ViewModel.SearchFilter
{
    public class SearchSectionCriteriaViewModel
    {
        [FromQuery(Name = "keyword")]
        public string? Keyword { get; set; } // SEARCH CODE AND NAME

        [FromQuery(Name = "academicLevelId")]
        public Guid? AcademicLevelId { get; set; }

        [FromQuery(Name = "termId")]
        public Guid? TermId { get; set; }

        [FromQuery(Name = "facultyId")]
        public Guid? FacultyId { get; set; }

        [FromQuery(Name = "departmentId")]
        public Guid? DepartmentId { get; set; }

        [FromQuery(Name = "instructorId")]
        public Guid? InstructorId { get; set; }

        [FromQuery(Name = "sectionStatus")]
        public SectionStatus? SectionStatus { get; set; }

        [FromQuery(Name = "sectionType")]
        public SectionType? SectionType { get; set; }

        [FromQuery(Name = "isAvailable")]
        public bool? IsAvailable { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}