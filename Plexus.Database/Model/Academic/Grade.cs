using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plexus.Database.Model.Academic
{
	[Table("Grades")]
	public class Grade
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public decimal Weight { get; set; }

		public string Letter { get; set; }	

		public bool IsCalculateGPA { get; set; }

		public bool IsCalculateAccumulateCredit { get; set; }

		public bool IsCalculateRegistrationCredit { get; set; }

		public bool IsShowTranscript { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }
	}
}

