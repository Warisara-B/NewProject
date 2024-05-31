using System;
using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic
{
	public class CreateGradeViewModel
	{
        [JsonProperty("letter")]
		public string Letter { get; set; }

		[JsonProperty("weight")]
		public decimal Weight { get; set; }

		[JsonProperty("isCalculateGPA")]
		public bool IsCalculateGPA { get; set; }

        [JsonProperty("isCalculateAccumulateCredit")]
		public bool IsCalculateAccumulateCredit { get; set; }

		[JsonProperty("isCalculateRegistrationCredit")]
		public bool IsCalculateRegistrationCredit { get; set; }

		[JsonProperty("isShowTranscript")]
		public bool IsShowTranscript { get; set; }
	}

	public class GradeViewModel : CreateGradeViewModel
    {
        [JsonProperty("id")]
		public Guid Id { get; set; }

		[JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

		[JsonProperty("updatedAt")]
		public DateTime UpdatedAt { get; set; }
	}
}

