using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment.Scholarship
{
    [Table("StudentScholarshipReserveBudgets")]
    public class StudentScholarshipReserveBudget
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid StudentScholarshipId { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(StudentScholarshipId))]
        public virtual StudentScholarship StudentScholarship { get; set; }
    }
}

