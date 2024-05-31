using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO
{
    public class SearchCurriculumVersionCriteriaDTO
    {
        public string? Name { get; set; }

        public string? Code { get; set; }

        public Guid? CurriculumId { get; set; }

        public bool? IsActive { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder OrderBy { get; set; } = SortingOrder.ASC;
    }
}