using Newtonsoft.Json;

namespace Plexus.Service.ViewModel
{
    public class NewsCategoryViewModel
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public class NewsFilterViewModel
	{
		public IEnumerable<Guid> CategoryIds { get; set; }

		public bool? IsPinned { get; set; }

		public bool? IsBookmarked { get; set; }
	}

	public class NewsListViewModel
	{
		[JsonProperty("id")]
		public Guid Id { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("thumbnailUrl")]
		public string? ThumbnailUrl { get; set; }

		[JsonProperty("publisher")]
		public string? Publisher { get; set; }

		[JsonProperty("publishAt")]
		public DateTime PublishAt { get; set; }

		[JsonProperty("isPinned")]
		public bool IsPinned { get; set; }

		[JsonProperty("isBookmarked")]
		public bool IsBookmarked { get; set; }
	}

	public class NewsDetailViewModel
	{
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("publisher")]
        public string? Publisher { get; set; }

        [JsonProperty("publishAt")]
        public DateTime PublishAt { get; set; }

        [JsonProperty("isPinned")]
        public bool IsPinned { get; set; }

        [JsonProperty("isBookmarked")]
        public bool IsBookmarked { get; set; }

		[JsonProperty("contentImageUrls")]
		public IEnumerable<string> ContentImageUrls = Enumerable.Empty<string>();
    }
}

