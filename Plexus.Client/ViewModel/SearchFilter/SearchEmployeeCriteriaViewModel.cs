using Microsoft.AspNetCore.Mvc;
using Plexus.Database.Enum;
using Plexus.Utility.ViewModel;

namespace Plexus.Client.ViewModel.SearchFilter
{
    public class SearchEmployeeCriteriaViewModel
    {
        [FromQuery(Name = "code")]
        public string? Code { get; set; }

        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        [FromQuery(Name = "facultyId")]
        public Guid? FacultyId { get; set; }

        [FromQuery(Name = "departmentId")]
        public Guid? DepartmentId { get; set; }

        [FromQuery(Name = "type")]
        public EmployeeType? Type { get; set; }

        [FromQuery(Name = "isActive")]
        public bool? IsActive { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;
    }
}