using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Academic.Advising;
using Plexus.Database.Model.Academic.Advising;

namespace Plexus.Database.Model.Academic
{
	[Table("StudentTerms")]
	public class StudentTerm
	{
		public Guid StudentId { get; set; }

		public Guid TermId { get; set; }

		public Guid? AdvisorId { get; set; }

		public decimal TotalCredit { get; set; }

		public decimal TotalRegistrationCredit { get; set; }

		public decimal? GPAX { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public AdvisingStatus Status { get; set; }

		public decimal? MinCredit { get; set; }

		public decimal? MaxCredit { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(StudentId))]
		public virtual Student Student { get; set; }

		[ForeignKey(nameof(TermId))]
		public virtual Term Term { get; set; }

		[ForeignKey(nameof(AdvisorId))]
		public virtual Employee? Advisor { get; set; }
	}
}