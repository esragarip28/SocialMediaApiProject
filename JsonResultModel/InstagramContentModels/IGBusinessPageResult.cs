using Newtonsoft.Json;
namespace DemoApplication1.ResultJsonClass
{
    public class IGBusinessPageResult
    {

        [JsonProperty("instagram_business_account")]
        public InstagramBusinessAccount InstagramBusinessAccount { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

        public class InstagramBusinessAccount
        {
            [JsonProperty("id")]
            public string Id { get; set; }
        }
}

