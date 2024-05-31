using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO
{
    public class SearchAcademicSpecializationCriteriaDTO
    {
        public string? Keyword { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public int? Level { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}