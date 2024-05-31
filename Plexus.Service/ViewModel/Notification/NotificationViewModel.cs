using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.ViewModel.Notification
{
    public class NotificationViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        [EnumDataType(typeof(NotificationType))]
        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationType Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("publishAt")]
        public DateTime PublishAt { get; set; }

        [JsonProperty("isRead")]
        public bool IsRead { get; set; }

        [JsonProperty("deepLink")]
        public string? DeepLink { get; set; }
    }

    public class CountNotificationViewModel
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
