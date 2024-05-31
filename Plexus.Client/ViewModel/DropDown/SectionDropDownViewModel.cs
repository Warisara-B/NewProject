using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.DropDown
{
	public class SectionDropDownViewModel : BaseDropDownViewModel
	{
        [JsonProperty("number")]
		public string Number { get; set; }

        [JsonProperty("courseId")]
		public Guid CourseId { get; set; }
		
        [JsonProperty("termId")]
		public Guid TermId { get; set; }
	}
}

