using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.DropDown
{
    public class BaseDropDownViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }   

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}