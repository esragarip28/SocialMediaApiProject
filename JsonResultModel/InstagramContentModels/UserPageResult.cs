using Newtonsoft.Json;
namespace DemoApplication1.ResultJsonClass
{
    public  class UserPageResult
    {   [JsonProperty("data")]
        public Data[] Data { get; set; }
    }
    public class Data
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("category_list")]
        public CategoryList[] CategoryList { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tasks")]
        public string[] Tasks { get; set; }
    }

    public  class CategoryList
    {
        [JsonProperty("id")]
      
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}

