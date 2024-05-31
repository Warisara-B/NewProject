using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum.Academic;

namespace Plexus.Client.ViewModel.Academic
{
	public class CreateTermViewModel
	{

		[JsonProperty("year")]
		public int Year { get; set; }

		[JsonProperty("term")]
		public string Number { get; set; }

		[JsonProperty("type")]
		[JsonConverter(typeof(StringEnumConverter))]
		public TermType Type { get; set; }

		[JsonProperty("collegeCalendarType")]
		[JsonConverter(typeof(StringEnumConverter))]
		public CollegeCalendarType CollegeCalendarType { get; set; }

		[JsonProperty("academicLevelId")]
		public Guid AcademicLevelId { get; set; }

		[JsonProperty("startedAt")]
		public DateTime StartedAt { get; set; }

		[JsonProperty("endedAt")]
		public DateTime EndedAt { get; set; }

		[JsonProperty("isCurrent")]
		public bool IsCurrent { get; set; }

		[JsonProperty("isRegistration")]
		public bool IsRegistration { get; set; }

		[JsonProperty("isAdvising")]
		public bool IsAdvising { get; set; }

		[JsonProperty("isSurveyed")]
		public bool IsSurveyed { get; set; }

		[JsonProperty("totalWeeks")]
		public int TotalWeeks { get; set; }
	}

	public class TermViewModel : CreateTermViewModel
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
	}
}

