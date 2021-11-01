using Newtonsoft.Json;

namespace DemoApplication1.Models.Entities.Concretes
{
    public class StoryDataResult
    {
        [JsonProperty("stories")]
        public Stories Stories { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Stories
    {
        [JsonProperty("data")]
        public StoryData[] Data { get; set; }

        [JsonProperty("paging")]
        public Paging Paging { get; set; }
    }

    public  class StoryData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Paging
    {
        [JsonProperty("cursors")]
        public Cursors Cursors { get; set; }
    }

    public  class Cursors
    {
        [JsonProperty("before")]
        public string Before { get; set; }

        [JsonProperty("after")]
        public string After { get; set; }
    }
}


