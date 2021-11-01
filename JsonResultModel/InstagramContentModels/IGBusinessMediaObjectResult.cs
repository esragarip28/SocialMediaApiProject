using Newtonsoft.Json;
namespace DemoApplication1.ResultJsonClass
{
    public class IGBusinessMediaObjectResult
    {

        [JsonProperty("data")]
        public MediaData[] Data { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }

    public partial class MediaData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Paging
    {
        [JsonProperty("cursors")]
        public Cursors Cursors { get; set; }
    }

    public partial class Cursors
    {
        [JsonProperty("before")]
        public string Before { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }
    }

}

