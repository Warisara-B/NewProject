using Plexus.Database.Enum.Academic.Advising;

namespace Plexus.Entity.DTO.Academic
{
	public class UpdateStudentTermDTO
	{
		public Guid TermId { get; set; }

		public AdvisingStatus Status { get; set; }

		public decimal? MinCredit { get; set; }

		public decimal? MaxCredit { get; set; }
	}

	public class StudentTermDTO : UpdateStudentTermDTO
	{
		public decimal TotalCredit { get; set; }

		public decimal TotalRegistrationCredit { get; set; }

		public decimal? GPAX { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}

