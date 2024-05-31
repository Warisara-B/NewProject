using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment.Scholarship
{
    [Table("StudentScholarships")]
    public class StudentScholarship
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public Guid ScholarShipId { get; set; }

        public decimal StartedLimitBalance { get; set; }

        public int StartTerm { get; set; }

        public int StartYear { get; set; }

        public int EndTerm { get; set; }

        public int EndYear { get; set; }

        public bool IsSendContract { get; set; }

        public bool IsActive { get; set; }

        public string? Remark { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public Guid? ApprovedBy { get; set; }

        public string? ApprovalRemark { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(ScholarShipId))]
        public virtual Scholarship Scholarship { get; set; }

        public virtual IEnumerable<StudentScholarshipReserveBudget> ReserveBudgets { get; set; }

        public virtual IEnumerable<StudentScholarshipUsage> Usages { get; set; }
    }
}

