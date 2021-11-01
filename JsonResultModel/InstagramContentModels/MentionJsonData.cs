using Newtonsoft.Json;
using System;

namespace DemoApplication1.ResultJsonClass
{
    public class MentionJsonData
    {
        [JsonProperty("data")]
        public MentionsData[] Data { get; set; }
    }

    public partial class MentionsData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("media_url")]
        public Uri MediaUrl { get; set; }

        [JsonProperty("comments_count")]
        public long CommentsCount { get; set; }

        [JsonProperty("like_count")]
        public long LikeCount { get; set; }

        [JsonProperty("permalink")]
        public Uri Permalink { get; set; }

        [JsonProperty("media_product_type")]
        public string MediaProductType { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
