using Microsoft.AspNetCore.Mvc;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.ViewModel.SearchFilter
{
    public class SearchRoomCriteriaViewModel
    {
        [FromQuery(Name = "keyword")]
        public string? Keyword { get; set; }

        [FromQuery(Name = "buildingId")]
        public Guid? BuildingId { get; set; }

        [FromQuery(Name = "campusId")]
        public Guid? CampusId { get; set; }

        [FromQuery(Name = "isActive")]
        public bool? IsActive { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder? OrderBy { get; set; }
    }
}