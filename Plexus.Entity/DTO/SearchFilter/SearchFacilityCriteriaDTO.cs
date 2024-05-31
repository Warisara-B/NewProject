using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO.SearchFilter
{
    public class SearchFacilityCriteriaDTO
    {
        public string? Keyword { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder? OrderBy { get; set; }
    }
}