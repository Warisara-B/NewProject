using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Plexus.Database.Enum.Registration;
using Plexus.Database.Model.Academic;

namespace Plexus.Database.Model.Registration
{
	[Table("Periods")]
	public class Period
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

		public string Name { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		[Column(TypeName = "nvarchar(100)")]
		public PeriodType Type { get; set; }

		public Guid TermId { get; set; }

		public DateTime CreatedAt { get; set; }

		public string CreatedBy { get; set; }

		public DateTime UpdatedAt { get; set; }

		public string UpdatedBy { get; set; }

		[ForeignKey(nameof(TermId))]
		public virtual Term Term { get; set; }

		public virtual IEnumerable<Slot> Slots { get; set; }
	}
}

