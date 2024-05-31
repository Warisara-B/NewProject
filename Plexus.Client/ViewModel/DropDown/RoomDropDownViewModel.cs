using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.DropDown
{
	public class RoomDropDownViewModel : BaseDropDownViewModel
	{
        [JsonProperty("buildingId")]
		public Guid? BuildingId { get; set; }
	}
}

