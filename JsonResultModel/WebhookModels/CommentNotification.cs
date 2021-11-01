using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.ResultJsonClass.WebhookModels
{
    public class CommentNotification
    {
        [JsonProperty("entry")]
        public CommentEntry[] Entry { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }
    }

    public  class CommentEntry
    {
        [JsonProperty("changes")]
        public CommentChange[] Changes { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }
    }

    public  class CommentChange
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("value")]
        public CommentValue Value { get; set; }
    }

    public class CommentValue
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}

