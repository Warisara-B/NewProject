using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic;

namespace Plexus.Database.Model.Academic
{
	[Table("Terms")]
	public class Term
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public int Year { get; set; }

		public string Number { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public TermType Type { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public CollegeCalendarType CollegeCalendarType { get; set; } = CollegeCalendarType.SEMESTER;

		public Guid AcademicLevelId { get; set; }

		public DateTime StartedAt { get; set; }

		public DateTime EndedAt { get; set; }

		public bool IsCurrent { get; set; }

		public bool IsRegistration { get; set; }

		public bool IsAdvising { get; set; }

		public bool IsSurveyed { get; set; }

		public int TotalWeeks { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(AcademicLevelId))]
		public virtual AcademicLevel AcademicLevel { get; set; }
	}
}

