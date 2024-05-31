using Newtonsoft.Json;

namespace Plexus.Service.ViewModel.Advising
{
    public class AdvisingViewModel
    {
        [JsonProperty("term")]
        public AdvisingTermViewModel Term { get; set; } = null!;

        [JsonProperty("status")]
        public AdvisingStatusViewModel Status { get; set; } = null!;

        [JsonProperty("courses")]
        public IEnumerable<AdvisingCourseViewModel> Courses { get; set; } = Enumerable.Empty<AdvisingCourseViewModel>();
    }

    public class AdvisingTermViewModel
    {
        [JsonProperty("academicTerm")]
        public string AcademicTerm { get; set; }

        [JsonProperty("academicYear")]
        public int AcademicYear { get; set; }
    }

    public class AdvisingStatusViewModel
    {
        [JsonProperty("isAdvised")]
        public bool IsAdvised { get; set; }

        [JsonProperty("isAllowRegistration")]
        public bool IsAllowRegistration { get; set; }

        [JsonProperty("isAllowPayment")]
        public bool IsAllowPayment { get; set; }
    }

    public class AdvisingCourseViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("credit")]
        public decimal Credit { get; set; }

        [JsonProperty("passGrade")]
        public string? PassGrade { get; set; }

        [JsonProperty("isRequired")]
        public bool IsRequired { get; set; }
    }
}