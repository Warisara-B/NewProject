namespace Plexus.Client.ViewModel.DropDown
{
    public class CourseDropDownViewModel : BaseDropDownViewModel
    {
        public Guid? AcademicLevelId { get; set; }
        
        public Guid? FacultyId { get; set; }

        public Guid? DepartmentId { get; set; }

        public string Code { get; set; }
    }
}