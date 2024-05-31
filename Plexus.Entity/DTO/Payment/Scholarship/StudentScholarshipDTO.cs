using System;
namespace Plexus.Entity.DTO.Payment.Scholarship
{
    public class CreateStudentScholarshipDTO
    {
        public Guid StudentId { get; set; }

        public Guid ScholarshipId { get; set; }

        public decimal StartedLimitBalance { get; set; }

        public int StartTerm { get; set; }

        public int StartYear { get; set; }

        public int EndTerm { get; set; }

        public int EndYear { get; set; }

        public string? Remark { get; set; }

        public bool IsSendContract { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<CreateStudentScholarshipReserveBudgetDTO> ReserveBudgets { get; set; }
    }

    public class CreateStudentScholarshipReserveBudgetDTO
    {
        public int? Term { get; set; }

        public int? Year { get; set; }
        
        public string Name { get; set; }

        public decimal Amount { get; set; }
    }

    public class StudentScholarshipReserveBudgetDTO : CreateStudentScholarshipReserveBudgetDTO
    {
        public Guid? Id { get; set; }

        public Guid ScholarshipId { get; set; }

        public decimal RemainingAmount { get; set; }
    }

    public class UpdateScholarshipReservedBudgetDTO
    {
        public Guid StudentScholarshipId { get; set; }

        public Guid ReservedBudgetId { get; set; }

        public int? Term { get; set; }

        public int? Year { get; set; }

        public decimal Amount { get; set; }
    }

    public class AdjustScholarshipBudgetDTO : UpdateScholarshipReservedBudgetDTO
    {
        public new Guid? ReservedBudgetId { get; set; }

        public string? Remark { get; set; }
    }

    public class UseScholarshipBudgetDTO : UpdateScholarshipReservedBudgetDTO
    {
        public string? Remark { get; set; }
    }

    public class StudentScholarshipDTO : CreateStudentScholarshipDTO
    {
        public Guid Id { get; set; }

        public new IEnumerable<StudentScholarshipReserveBudgetDTO> ReserveBudgets { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public Guid? ApprovedBy { get; set; }

        public string? ApprovalRemark { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

