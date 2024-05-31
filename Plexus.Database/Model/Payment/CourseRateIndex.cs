using Plexus.Database.Enum.Payment;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment
{
    [Table("CourseRateIndexes")]
    public class CourseRateIndex
    {
        public Guid CourseRateId { get; set; }

        public Guid RateTypeId { get; set; }

        public int Index { get; set; }

        public decimal Amount { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public CalculationType? CalculationType { get; set; }

        [ForeignKey(nameof(CourseRateId))]
        public virtual CourseRate CourseRate { get; set; }

        [ForeignKey(nameof(RateTypeId))]
        public virtual RateType RateType { get; set; }
    }
}

