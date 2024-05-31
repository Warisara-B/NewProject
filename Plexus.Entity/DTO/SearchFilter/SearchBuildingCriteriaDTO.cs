using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO.SearchFilter
{
    public class SearchBuildingCriteriaDTO
    {
        public string? Keyword { get; set; }

        public Guid? CampusId { get; set; }

        public bool? IsActive { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder? OrderBy { get; set; }
    }
}