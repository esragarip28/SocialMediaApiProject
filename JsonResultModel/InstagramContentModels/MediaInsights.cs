using Newtonsoft.Json;

namespace DemoApplication1.ResultJsonClass
{
    public class MediaInsights
    {
        [JsonProperty("data")]
        public MediaInsightsData[] Data { get; set; }
    }

    public  class MediaInsightsData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("values")]
        public NotificationValue[] Values { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public  class NotificationValue
    {
        [JsonProperty("value")]
        public long ValueValue { get; set; }
    }
}