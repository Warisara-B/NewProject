using Plexus.Utility.ViewModel;

namespace Plexus.Entity.DTO.SearchFilter
{
    public class SearchSlotConditionCriteriaDTO
    {
        public string? Keyword { get; set; }

        public Guid? SlotId { get; set; }

        public bool? IsActive { get; set; }

        public string? SortBy { get; set; }

        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}