using Newtonsoft.Json;

namespace DemoApplication1.ResultJsonClass
{
    public class ItemId
    {
        [JsonProperty("id")]
        public string id { get; set; }
    }
}
