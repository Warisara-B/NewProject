using System;
using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.DropDown
{
	public class BuildingDropDownViewModel : BaseDropDownViewModel
	{
        [JsonProperty("campusId")]
		public Guid? CampusId { get; set; }
	}
}

