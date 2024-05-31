using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.DropDown
{
	public class DepartmentDropDownViewModel : BaseDropDownViewModel
	{
        [JsonProperty("facultyId")]
		public Guid? FacultyId { get; set; }
	}
}