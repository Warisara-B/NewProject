using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.ViewModel.Notification
{
    public class NotificaitonDetailViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        [EnumDataType(typeof(NotificationType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationType Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("publishAt")]
        public DateTime? PublishAt { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("urlImgs")]
        public IEnumerable<string> UrlImgs { get; set; }
    }
}
