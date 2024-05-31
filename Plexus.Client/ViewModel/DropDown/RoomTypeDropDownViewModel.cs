using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.DropDown
{
	public class RoomTypeDropDownViewModel : BaseDropDownViewModel
	{
        [JsonProperty("name")]
		public string? Name { get; set; }
	}
}

