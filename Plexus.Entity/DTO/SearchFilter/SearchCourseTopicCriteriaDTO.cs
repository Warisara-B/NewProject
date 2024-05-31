using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO
{
    public class SearchCourseTopicCriteriaDTO
    {
        public string? Keyword { get; set; }

        public Guid? CourseId { get; set; }

        public bool? IsActive { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}