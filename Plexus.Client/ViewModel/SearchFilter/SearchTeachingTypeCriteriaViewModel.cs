using Microsoft.AspNetCore.Mvc;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.ViewModel.SearchFilter
{
    public class SearchTeachingTypeCriteriaViewModel
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        [FromQuery(Name = "isActive")]
        public bool? IsActive { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder? OrderBy { get; set; }
    }
}