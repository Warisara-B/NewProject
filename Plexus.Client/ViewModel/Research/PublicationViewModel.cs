using Newtonsoft.Json;

namespace Plexus.Client.ViewModel.Research
{
    public class CreatePublicationViewModel
    {
        [JsonProperty("articleTypeId")]
        public Guid ArticleTypeId { get; set; }

        [JsonProperty("authors")]
        public string Authors { get; set; }

        [JsonProperty("pages")]
        public int Pages { get; set; }

        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("citationPages")]
        public string? CitationPages { get; set; }

        [JsonProperty("citationDOI")]
        public string? CitationDOI { get; set; }
    }

    public class PublicationViewModel : CreatePublicationViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}