using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Registration;

namespace Plexus.Client.ViewModel.Registration
{
	public class CreatePeriodViewModel
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("fromDate")]
		public DateTime FromDate { get; set; }

		[JsonProperty("toDate")]
		public DateTime ToDate { get; set; }

		[JsonProperty("type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public PeriodType Type { get; set; }

		[JsonProperty("termId")]
		public Guid TermId { get; set; }
	}

	public class PeriodViewModel : CreatePeriodViewModel
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }

		/// <summary>
		/// Term academic level
		/// </summary>
		[JsonProperty("academicLevelName")]
		public string? AcademicLevelName { get; set; }

		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updatedAt")]
		public DateTime UpdatedAt { get; set; }

		[JsonProperty("slots")]
		public IEnumerable<SlotViewModel> Slots { get; set; }
	}
}

