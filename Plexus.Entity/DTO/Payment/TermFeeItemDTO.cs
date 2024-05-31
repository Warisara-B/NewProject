using Plexus.Database.Enum.Academic;
using Plexus.Database.Enum.Payment;

namespace Plexus.Entity.DTO.Payment
{
    public class CreateTermFeeItemDTO
    {
        public Guid TermFeePackageId { get; set; }

        public Guid FeeItemId { get; set; }

        public IEnumerable<TermType>? TermType { get; set; }
        
        public RecurringType RecurringType { get; set; }

        public TermFeeItemConditionDTO? Condition { get; set; }

        public decimal Amount { get; set; }

        public string Remark { get; set; }

		public bool IsActive { get; set; }
    }

    public class TermFeeItemDTO : CreateTermFeeItemDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class TermFeeItemConditionDTO
    {
        public Guid? AcademicLevelId { get; set; }

        public Guid? FacultyId { get; set; }

        public Guid? DepartmentId { get; set; }

        public Guid? CurriculumId { get; set; }

        public Guid? CurriculumVersionId { get; set; }

        public int? FromBatch { get; set; }

        public int? ToBatch { get; set; }

        public Guid? StudentFeeTypeId { get; set; }
    }
}