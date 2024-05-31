using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.DropDown
{
	public class FeeGroupDropDownViewModel : BaseDropDownViewModel
	{
        [JsonProperty("code")]
		public string Code { get; set; }
	}
}