using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Academic.Section
{
    public class CreateOfferedCourseViewModel : CreateSectionViewModel
    {
        [JsonProperty("seats")]
        public IEnumerable<UpsertSectionSeatViewModel>? Seats { get; set; }

        [JsonProperty("jointSections")]
        public IEnumerable<CreateJointSectionViewModel>? JointSections { get; set; }

        [JsonProperty("details")]
        public IEnumerable<UpdateSectionDetailViewModel> Details { get; set; }

        [JsonProperty("examinations")]
        public IEnumerable<UpdateSectionExaminationViewModel> Examinations { get; set; }
    }

    public class OfferedCourseViewModel : SectionViewModel
    {
        [JsonProperty("seats")]
        public IEnumerable<UpsertSectionSeatViewModel>? Seats { get; set; }

        [JsonProperty("jointSections")]
        public IEnumerable<JointSectionViewModel>? JointSections { get; set; }

        [JsonProperty("examinations")]
        public IEnumerable<SectionExaminationViewModel> Examinations { get; set; }
    }

    public class CreateJointSectionViewModel
    {
        [JsonProperty("courseId")]
        public Guid CourseId { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("seatLimit")]
        public int SeatLimit { get; set; }

        [JsonProperty("remark")]
        public string? Remark { get; set; }
    }
    
    public class JointSectionViewModel : CreateJointSectionViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("courseCode")]
        public string? CourseCode { get; set; }

        [JsonProperty("courseName")]
        public string? CourseName { get; set; }

        [JsonProperty("createdAt")]
		public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
		public DateTime UpdatedAt { get; set; }
    }
}