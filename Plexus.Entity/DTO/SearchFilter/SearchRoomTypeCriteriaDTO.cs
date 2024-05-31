using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO.SearchFilter
{
    public class SearchRoomTypeCriteriaDTO
    {
        public string? Keyword { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder? OrderBy { get; set; }
    }
}