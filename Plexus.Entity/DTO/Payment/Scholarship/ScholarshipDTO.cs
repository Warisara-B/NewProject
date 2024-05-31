namespace Plexus.Entity.DTO.Payment.Scholarship
{
    public class CreateScholarshipDTO
    {
        public Guid ScholarshipTypeId { get; set; }

        public string Sponsor { get; set; }

        public string Name { get; set; }

        public decimal TotalBudget { get; set; }

        public decimal LimitBalance { get; set; }

        public int YearDuration { get; set; }

        public decimal? MinGPA { get; set; }

        public decimal? MaxGPA { get; set; }

        public bool IsRepeatCourseApplied { get; set; }

        public bool IsAllCoverage { get; set; }

        public bool IsActive { get; set; }

        public string? Remark { get; set; }

        public IEnumerable<ScholarshipReserveBudgetDTO> Budgets { get; set; }

        public IEnumerable<ScholarshipFeeItemDTO> FeeItems { get; set; }
    }

    public class ScholarshipDTO : CreateScholarshipDTO
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }    
}