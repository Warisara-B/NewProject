using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment
{
    [Table("CourseRates")]
    public class CourseRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid RateTypeId { get; set; }

        public string Name { get; set; }

        public string? Conditions { get; set; }

        public int Index { get; set; }

        public decimal Amount { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(RateTypeId))]
        public virtual RateType RateType { get; set; }
    }
}

