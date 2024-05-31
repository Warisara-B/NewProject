using Plexus.Database.Enum.Payment;

namespace Plexus.Entity.DTO.Payment
{
    public class CreateCourseFeeDTO
    {
        public Guid CourseId { get; set; }

        public Guid FeeItemId { get; set; }

        public CalculationType CalculationType { get; set; }

        public CourseFeeConditionDTO? Condition { get; set; }
        
        public Guid RateTypeId { get; set; }

        public int? RateIndex { get; set; }
    }   

    public class CourseFeeDTO : CreateCourseFeeDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class CourseFeeConditionDTO
    {
        public string? SectionNumber { get; set; }

        public Guid? AcademicLevelId { get; set; }

        public int? FromBatch { get; set; }

        public int? ToBatch { get; set; }

        public Guid? FacultyId { get; set; }

        public Guid? DepartmentId { get; set; }

        public Guid? CurriculumId { get; set; }

        public Guid? CurriculumVersionId { get; set; }

        public Guid? StudentFeeTypeId { get; set; }
    }
}