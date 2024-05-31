using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO
{
    public class SearchCourseCriteriaDTO
    {
        public string? Keyword { get; set; }

        public Guid? AcademicLevelId { get; set; }

        public Guid? FacultyId { get; set; }

        public Guid? DepartmentId { get; set; }

        public bool? IsActive { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}