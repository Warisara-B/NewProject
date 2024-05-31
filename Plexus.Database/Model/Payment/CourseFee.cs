using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Payment;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Payment
{
    [Table("CourseFees")]
    public class CourseFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

        public Guid CourseId { get; set; }

        public Guid FeeItemId { get; set; }

        public string? Conditions { get; set; }
        
        public Guid RateTypeId { get; set; }

        public int? RateIndex { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public CalculationType CalculationType { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        [ForeignKey(nameof(CourseId))]
		public virtual Course Course { get; set; }

        [ForeignKey(nameof(FeeItemId))]
		public virtual FeeItem FeeItem { get; set; }

        [ForeignKey(nameof(RateTypeId))]
		public virtual RateType RateType { get; set; }
    }
}