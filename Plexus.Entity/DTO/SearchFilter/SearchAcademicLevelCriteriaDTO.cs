using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO.SearchFilter
{
    public class SearchAcademicLevelCriteriaDTO
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool? IsActive { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}