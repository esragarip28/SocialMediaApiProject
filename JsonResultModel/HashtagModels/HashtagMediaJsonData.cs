using System;
using Newtonsoft.Json;
namespace DemoApplication1.ResultJsonClass
{
    public class HashtagMediaJsonData
    {

        [JsonProperty("data")]
        public HashtagData[] Data { get; set; }

        [JsonProperty("paging")]
        public HashtagPaging Paging { get; set; }
    }

    public partial class HashtagData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("comments_count")]
        public int CommentsCount { get; set; }

        [JsonProperty("like_count")]
        public int LikeCount { get; set; }

        [JsonProperty("media_url")]
        public Uri MediaUrl { get; set; }

        [JsonProperty("permalink")]
        public Uri Permalink { get; set; }
    }

    public partial class HashtagPaging
    {
        [JsonProperty("cursors")]
        public HashtagCursors Cursors1 { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }
    }

    public partial class HashtagCursors
    {
        [JsonProperty("after")]
        public string After { get; set; }
    }

}