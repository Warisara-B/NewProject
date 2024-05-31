using Microsoft.AspNetCore.Mvc;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.ViewModel.SearchFilter
{
    public class SearchCourseTopicCriteriaViewModel
    {
        [FromQuery(Name = "keyword")]
        public string? Keyword { get; set; }

        [FromQuery(Name = "courseId")]
        public Guid? CourseId { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder OrderBy { get; set; } = SortingOrder.ASC;
    }
}