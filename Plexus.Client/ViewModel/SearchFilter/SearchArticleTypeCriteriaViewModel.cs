using Microsoft.AspNetCore.Mvc;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO.SearchFilter
{
    public class SearchArticleTypeCriteriaViewModel
    {
        [FromQuery(Name = "keyword")]
        public string? Keyword { get; set; }

        [FromQuery(Name = "isActive")]
        public bool? IsActive { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}