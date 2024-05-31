using System;
namespace Plexus.Entity.DTO.Registration
{
	public class CreateSlotDTO
	{
		public Guid PeriodId { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public DateTime StartedAt { get; set; }

		public DateTime EndedAt { get; set; }

		public bool IsActive { get; set; }

		public bool IsSpecialSlot { get; set; }
	}

	public class SlotDTO : CreateSlotDTO
	{
		public Guid Id { get; set; }

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}
}

