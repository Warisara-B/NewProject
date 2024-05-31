using Newtonsoft.Json;
using Plexus.Database.Enum.Academic.Advising;

namespace Plexus.Client.ViewModel.Academic
{
    public class UpdateStudentTermViewModel
    {
        [JsonProperty("termId")]
        public Guid TermId { get; set; }

        [JsonProperty("status")]
        public AdvisingStatus Status { get; set; }

        [JsonProperty("minCredit")]
        public decimal? MinCredit { get; set; }

        [JsonProperty("maxCredit")]
        public decimal? MaxCredit { get; set; }
    }

    public class StudentTermViewModel : UpdateStudentTermViewModel
    {

        [JsonProperty("totalCredit")]
        public decimal TotalCredit { get; set; }

        [JsonProperty("totalRegistrationCredit")]
        public decimal TotalRegistrationCredit { get; set; }

        [JsonProperty("gpax")]
        public decimal? GPAX { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}