using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Payment.Scholarship;

namespace Plexus.Database.Model.Payment.Scholarship
{
    [Table("StudentScholarshipUsages")]
    public class StudentScholarshipUsage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public Guid StudentScholarshipId { get; set; }

        public Guid? ReserveBudgetId { get; set; }

        public int? Year { get; set; }

        public int? Term { get; set; }

        public string? DocumentNumber { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public ScholarshipUsageAction Action { get; set; }

        public decimal Amount { get; set; }

        public string? Remark { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        [ForeignKey(nameof(StudentScholarshipId))]
        public virtual StudentScholarship StudentScholarship { get; set; }

        [ForeignKey(nameof(ReserveBudgetId))]
        public virtual StudentScholarshipReserveBudget? ReserveBudget { get; set; }
    }
}