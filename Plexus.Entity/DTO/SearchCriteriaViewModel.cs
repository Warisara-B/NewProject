using Microsoft.AspNetCore.Mvc;
using Plexus.Database.Enum;
using Plexus.Utility.ViewModel;
using Plexus.Database.Enum.Facility.Reservation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Payment;

namespace Plexus.Entity.DTO
{
    public class SearchCriteriaViewModel
    {
        [FromQuery(Name = "keyword")]
        public string? Keyword { get; set; }

        [FromQuery(Name = "academicLevelId")]
        public Guid? AcademicLevelId { get; set; }

        [FromQuery(Name = "termId")]
        public Guid? TermId { get; set; }

        [FromQuery(Name = "facultyId")]
        public Guid? FacultyId { get; set; }

        [FromQuery(Name = "departmentId")]
        public Guid? DepartmentId { get; set; }

        [FromQuery(Name = "code")]
        public string? Code { get; set; }

        [FromQuery(Name = "name")]
        public string? Name { get; set; }

        [FromQuery(Name = "campusId")]
        public Guid? CampusId { get; set; }

        [FromQuery(Name = "curriculumId")]
        public Guid? CurriculumId { get; set; }

        [FromQuery(Name = "curriculumVersionId")]
        public Guid? CurriculumVersionId { get; set; }

        [FromQuery(Name = "buildingId")]
        public Guid? BuildingId { get; set; }

        [FromQuery(Name = "courseId")]
        public Guid? CourseId { get; set; }

        [FromQuery(Name = "isMaster")]
        public bool? IsMasterSection { get; set; }

        [FromQuery(Name = "roomId")]
        public Guid? RoomId { get; set; }

        [FromQuery(Name = "number")]
        public string? Number { get; set; }

        [FromQuery(Name = "term")]
        public string? Term { get; set; }

        [FromQuery(Name = "year")]
        public int? Year { get; set; }

        [FromQuery(Name = "capacity")]
        public int? Capacity { get; set; }

        [FromQuery(Name = "scholarshipTypeId")]
        public Guid? ScholarshipTypeId { get; set; }

        [FromQuery(Name = "sponsor")]
        public string? Sponsor { get; set; }

        [FromQuery(Name = "instructorTypeId")]
        public Guid? InstructorTypeId { get; set; }

        [FromQuery(Name = "isActive")]
        public bool? IsActive { get; set; }

        [FromQuery(Name = "isClosed")]
        public bool? IsClosed { get; set; }

        [FromQuery(Name = "fromDate")]
        public DateTime? FromDate { get; set; }

        [FromQuery(Name = "toDate")]
        public DateTime? ToDate { get; set; }

        [FromQuery(Name = "floor")]
        public int? Floor { get; set; }

        [FromQuery(Name = "senderType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public SenderType? SenderType { get; set; }

        [FromQuery(Name = "createdBy")]
        public string? CreatedBy { get; set; }

        [FromQuery(Name = "slotId")]
        public Guid? SlotId { get; set; }

        [FromQuery(Name = "scholarshipId")]
        public Guid? ScholarshipId { get; set; }

        [FromQuery(Name = "studentId")]
        public Guid? StudentId { get; set; }

        [FromQuery(Name = "minimumGPA")]
        public decimal? MinimumGPA { get; set; }

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "orderBy")]
        public SortingOrder? OrderBy { get; set; } = SortingOrder.ASC;

        [FromQuery(Name = "withdrawalStatus")]
        public WithdrawalStatus? WithdrawalStatus { get; set; }

        [FromQuery(Name = "termFeePackageType")]
        public TermFeePackageType? TermFeePackageType { get; set; }

        [FromQuery(Name = "feeGroupId")]
        public Guid? FeeGroupId { get; set; }

        [FromQuery(Name = "feeItemId")]
        public Guid? FeeItemId { get; set; }

        [FromQuery(Name = "termType")]
        public TermType? TermType { get; set; }

        [FromQuery(Name = "recurringType")]
        public RecurringType? RecurringType { get; set; }
    }
}
