using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO.SearchFilter
{
    public class SearchPublicationCriteriaDTO
    {
        public string? Keyword { get; set; }

        public Guid? ArticleTypeId { get; set; }

        public int? Year { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}