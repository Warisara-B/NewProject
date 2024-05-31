using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.DropDown
{
	public class InstructorDropDownViewModel : BaseDropDownViewModel
	{
        [JsonProperty("code")]
		public string? Code { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

        [JsonProperty("middleName")]
		public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
		public string LastName { get; set; }
	}
}