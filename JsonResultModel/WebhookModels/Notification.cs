using Newtonsoft.Json;
namespace DemoApplication1.ResultJsonClass
{
    public class Notification
    {

        [JsonProperty("entry")]
        public Entry[] Entry { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }
    }

    public  class Entry
    {
        [JsonProperty("changes")]
        public Change[] Changes { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }
    }

    public partial class Change
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public  class Value
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("media_id")]
        public string MediaId { get; set; }

        [JsonProperty("comment_id")]
        public string CommentId { get; set; }

        [JsonProperty("impressions")]
        public long Impressions { get; set; }

        [JsonProperty("reach")]
        public long Reach { get; set; }

        [JsonProperty("taps_forward")]
        public long TapsForward { get; set; }

        [JsonProperty("taps_back")]
        public long TapsBack { get; set; }

        [JsonProperty("exits")]
        public long Exits { get; set; }

        [JsonProperty("replies")]
        public long Replies { get; set; }

        [JsonProperty("sender")]
        public Recipient Sender { get; set; }

        [JsonProperty("recipient")]
        public Recipient Recipient { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("postback")]
        public Postback Postback { get; set; }
    }

    public  class Postback
    {
        [JsonProperty("mid")]
        public string Mid { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }

    public  class Recipient
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}

