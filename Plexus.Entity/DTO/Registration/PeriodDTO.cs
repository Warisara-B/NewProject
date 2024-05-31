using System;
using Plexus.Database.Enum.Registration;

namespace Plexus.Entity.DTO.Registration
{
	public class CreatePeriodDTO
    {
		public string Name { get; set; }

		public DateTime FromDate { get; set; }

		public DateTime ToDate { get; set; }

		public PeriodType Type { get; set; }

		public Guid TermId { get; set; }
	}

	public class PeriodDTO : CreatePeriodDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }

		public IEnumerable<SlotDTO> Slots { get; set; } = Enumerable.Empty<SlotDTO>();
	}
}

