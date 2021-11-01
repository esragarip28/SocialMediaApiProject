using Newtonsoft.Json;
using System;

namespace DemoApplication1.ResultJsonClass
{
    public class AccountInsights
    {
        [JsonProperty("data")]
        public AccountInsightsData[] Data { get; set; }

        [JsonProperty("paging")]
        public AccountInsightsPaging Paging { get; set; }
    }

    public  class AccountInsightsData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("period")]
        public string Period { get; set; }

        [JsonProperty("values")]
        public AccountInsightsValue[] Values { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public  class AccountInsightsValue
    {
        [JsonProperty("value")]
        public int Val { get; set; }

        [JsonProperty("end_time")]
        public string EndTime { get; set; }
    }

    public  class AccountInsightsPaging
    {
        [JsonProperty("previous")]
        public Uri Previous { get; set; }

        [JsonProperty("next")]
        public Uri Next { get; set; }
    }
}

