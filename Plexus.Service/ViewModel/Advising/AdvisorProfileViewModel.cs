using Newtonsoft.Json;

namespace Plexus.Service.ViewModel.Advising
{
    public class AdvisorProfileViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string? MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("profileImageUrl")]
        public string? ProfileImageUrl { get; set; }

        [JsonProperty("faculty")]
        public AdvisorFacultyViewModel Faculty { get; set; }

        [JsonProperty("academicContact")]
        public AdvisorAcademicContactViewModel AcademicContact { get; set; }

        [JsonProperty("personalContact")]
        public AdvisorPersonalContactViewModel PersonalContact { get; set; }
    }

    public class AdvisorFacultyViewModel
    {
        [JsonProperty("logoUrl")]
        public string? LogoUrl { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("departmentName")]
        public string? DepartmentName { get; set; }
    }

    public class AdvisorAcademicContactViewModel
    {
        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }
    }

    public class AdvisorPersonalContactViewModel
    {
        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }
    }
}