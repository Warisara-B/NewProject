using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Registration
{
	public class CreateSlotViewModel
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("description")]
		public string? Description { get; set; }

		[JsonProperty("startedAt")]
		public DateTime StartedAt { get; set; }

		[JsonProperty("endedAt")]
		public DateTime EndedAt { get; set; }

		[JsonProperty("isActive")]
		public bool IsActive { get; set; }

		[JsonProperty("isSpecialSlot")]
		public bool IsSpecialSlot { get; set; }
	}

	public class SlotViewModel : CreateSlotViewModel
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }

		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updatedAt")]
		public DateTime UpdatedAt { get; set; }
	}
}

