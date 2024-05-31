using System;
namespace Plexus.Entity.DTO.Academic
{
	public class CreateGradeDTO
	{
		public decimal Weight { get; set; }

		public string Letter { get; set; }

		public bool IsCalculateGPA { get; set; }

		public bool IsCalculateAccumulateCredit { get; set; }

		public bool IsCalculateRegistrationCredit { get; set; }

		public bool IsShowTranscript { get; set; }
	}

	public class GradeDTO : CreateGradeDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}

