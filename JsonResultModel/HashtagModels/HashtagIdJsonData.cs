using Newtonsoft.Json;

namespace DemoApplication1.ResultJsonClass
{
    public class HashtagIdJsonData
    {
        [JsonProperty("data")]
        public Datum[] Data { get; set; }


    }

    public partial class Datum
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}

 
