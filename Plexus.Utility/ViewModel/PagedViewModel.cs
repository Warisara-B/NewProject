using Newtonsoft.Json;

namespace Plexus.Utility.ViewModel
{
    public class PagedViewModel<T> where T : class
    {
        /// <summary>
        /// Current page
        /// </summary>
        [JsonProperty("currentPage")]
        public int Page { get; set; }

        /// <summary>
        /// Current page Size
        /// </summary>
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Total page available
        /// </summary>
        [JsonProperty("totalPage")]
        public int TotalPage { get; set; }

        /// <summary>
        /// Total item count
        /// </summary>
        [JsonProperty("totalItem")]
        public int TotalItem { get; set; }

        /// <summary>
        /// List of items in current page
        /// </summary>
        [JsonProperty("items")]
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}

