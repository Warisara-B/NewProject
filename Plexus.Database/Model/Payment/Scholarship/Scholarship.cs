using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Payment.Scholarship
{
    [Table("Scholarships")]
    public class Scholarship
    {
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

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

        public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

        [ForeignKey(nameof(ScholarshipTypeId))]
		public virtual ScholarshipType ScholarshipType { get; set; }
    }
}