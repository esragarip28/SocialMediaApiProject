using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.ResultJsonClass.WebhookModels
{
    public class MentionNotification
    {
        [JsonProperty("entry")]
        public MentionEntry[] Entry { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }
    }

    public  class MentionEntry
    {
        [JsonProperty("changes")]
        public MentionChange[] Changes { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }
    }

    public  class MentionChange
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("value")]
        public MentionValue Value { get; set; }
    }

    public  class MentionValue
    {
        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        [JsonProperty("comment_id")]
        public string CommentId { get; set; }
    }
}

