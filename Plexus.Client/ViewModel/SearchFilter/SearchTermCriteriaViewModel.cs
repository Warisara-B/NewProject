using Microsoft.AspNetCore.Mvc;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.ViewModel.SearchFilter
{
    public class SearchTermCriteriaViewModel
    {
        [FromQuery(Name = "academicLevelId")]
        public Guid? AcademicLevelId { get; set; }

        [FromQuery(Name = "term")]
        public string? Term { get; set; }

        [FromQuery(Name = "year")]
        public int? Year { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}