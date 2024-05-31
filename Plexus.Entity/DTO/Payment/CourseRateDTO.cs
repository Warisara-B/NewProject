using Newtonsoft.Json;
using Plexus.Database.Enum.Payment;

namespace Plexus.Entity.DTO.Payment
{
    public class CreateCourseRateDTO
    {
        public string Description { get; set; }

        public CourseRateConditionDTO? Condition { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<CourseRateIndexDTO> Indexes { get; set; }
    }

    public class CourseRateDTO : CreateCourseRateDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CourseRateConditionDTO
    {
        [JsonProperty("academicLevelId")]
        public Guid? AcademicLevelId { get; set; }

        [JsonProperty("fromBatch")]
        public int? FromBatch { get; set; }

        [JsonProperty("toBatch")]
        public int? ToBatch { get; set; }

        [JsonProperty("facultyId")]
        public Guid? FacultyId { get; set; }

        [JsonProperty("departmentId")]
        public Guid? DepartmentId { get; set; }

        [JsonProperty("curriculumId")]
        public Guid? CurriculumId { get; set; }

        [JsonProperty("curriculumVersionId")]
        public Guid? CurriculumVersionId { get; set; }

        [JsonProperty("studentFeeTypeId")]
        public Guid? StudentFeeTypeId { get; set; }
    }

    public class CourseRateIndexDTO
    {
        public Guid RateTypeId { get; set; }

        public int Index { get; set; }

        public decimal Amount { get; set; }

        public CalculationType? CalculationType { get; set; }
    }
}

