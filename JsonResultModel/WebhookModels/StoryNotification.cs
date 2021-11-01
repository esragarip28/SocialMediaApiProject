using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication1.ResultJsonClass
{
    public class StoryNotification
    {
        [JsonProperty("entry")]
        public StoryEntry[] Entry { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }
    }

    public partial class StoryEntry
    {
        [JsonProperty("changes")]
        public StoryChange[] Changes { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }
    }

    public partial class StoryChange
    {
        [JsonProperty("field")]
        public string Field { get; set; }

        [JsonProperty("value")]
        public StoryValue Value { get; set; }
    }

    public partial class StoryValue
    {
        [JsonProperty("media_id")]
        public string MediaId { get; set; }

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
    }
}

 
