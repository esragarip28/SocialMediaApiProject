using Newtonsoft.Json;
using System;

namespace DemoApplication1.Models.Entities.Concretes
{
    public class Story
    {
        [JsonProperty("media_url")]
        public Uri MediaUrl { get; set; }

        [JsonProperty("permalink")]
        public Uri Permalink { get; set; }

        [JsonProperty("media_product_type")]
        public string MediaProductType { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
